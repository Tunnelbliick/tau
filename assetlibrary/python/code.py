from svgpathtools import parse_path
from svgwrite import Drawing
from PIL import Image
import cairosvg
import os

path_data = "M145.5 142.5V10H290V140.5L331.5 103.5L416 190.5L216 391L15 189L101.5 103L145.5 142.5Z"
output_dir = "frames_svg"
frame_count = 60
img_size = (450, 420)

os.makedirs(output_dir, exist_ok=True)
path = parse_path(path_data)
length = path.length()

for i in range(1, frame_count + 1):
    partial = path.cropped(0, length * i / frame_count)
    svg_path = partial.d()

    # Build SVG with background
    dwg = Drawing(size=img_size)
    dwg.add(dwg.rect(insert=(0, 0), size=img_size, fill="black"))
    dwg.add(dwg.path(d=svg_path, stroke="white", fill="none", stroke_width=4))
    svg_filename = os.path.join(output_dir, f"frame_{i:03}.svg")
    png_filename = svg_filename.replace(".svg", ".png")

    dwg.saveas(svg_filename)

    # Use cairosvg only if your system can run it — otherwise skip
    try:
        cairosvg.svg2png(url=svg_filename, write_to=png_filename)
    except Exception as e:
        print("CairoSVG failed:", e)
