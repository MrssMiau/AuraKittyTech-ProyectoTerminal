"""
Vigila la carpeta de salida de KScan3D y abre cada malla nueva en Meshmixer.
Solo usa la biblioteca estándar (sin pip install).

Uso:
    python kscan_a_meshmixer.py
    python kscan_a_meshmixer.py --carpeta "D:/Scans/Salida" --meshmixer "C:/Program Files/Autodesk/Meshmixer/meshmixer.exe"
"""

import argparse
import subprocess
import sys
import time
from pathlib import Path

# ---------------- CONFIGURACIÓN (ajusta estas rutas) ----------------
CARPETA_SALIDA = r"C:\Users\TU_USUARIO\Documents\KScan3D\Export"
RUTA_MESHMIXER = r"C:\Program Files\Autodesk\Meshmixer\meshmixer.exe"
EXTENSIONES = {".obj", ".stl", ".ply"}   # formatos que Meshmixer abre
INTERVALO = 1.0                          # segundos entre revisiones
ESTABLE_SEG = 2.0                        # tiempo sin cambios para considerar el archivo terminado
# ---------------------------------------------------------------------


def esperar_a_que_termine(ruta: Path, estable_seg: float) -> bool:
    """Espera a que KScan3D termine de escribir (tamaño estable)."""
    ultimo = -1
    estable_desde = time.time()
    while True:
        try:
            tam = ruta.stat().st_size
        except FileNotFoundError:
            return False
        if tam != ultimo:
            ultimo = tam
            estable_desde = time.time()
        elif tam > 0 and time.time() - estable_desde >= estable_seg:
            # Verifica que no esté bloqueado por otro proceso
            try:
                with open(ruta, "ab"):
                    return True
            except OSError:
                estable_desde = time.time()
        time.sleep(0.5)


def listar(carpeta: Path) -> set[Path]:
    return {p for p in carpeta.iterdir() if p.is_file() and p.suffix.lower() in EXTENSIONES}


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("--carpeta", default=CARPETA_SALIDA)
    ap.add_argument("--meshmixer", default=RUTA_MESHMIXER)
    args = ap.parse_args()

    carpeta = Path(args.carpeta)
    exe = Path(args.meshmixer)

    if not carpeta.is_dir():
        sys.exit(f"No existe la carpeta: {carpeta}")
    if not exe.is_file():
        sys.exit(f"No encuentro Meshmixer en: {exe}")

    vistos = listar(carpeta)  # lo que ya existía se ignora
    print(f"Vigilando {carpeta} ... (Ctrl+C para salir)")

    try:
        while True:
            actuales = listar(carpeta)
            for nuevo in sorted(actuales - vistos):
                print(f"Nuevo archivo: {nuevo.name} -> esperando a que termine de escribirse")
                if esperar_a_que_termine(nuevo, ESTABLE_SEG):
                    print("Abriendo en Meshmixer...")
                    subprocess.Popen([str(exe), str(nuevo)])
                vistos.add(nuevo)
            time.sleep(INTERVALO)
    except KeyboardInterrupt:
        print("\nDetenido.")


if __name__ == "__main__":
    main()