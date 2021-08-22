Import("input.avs")

ConvertToRGB64()
ConvertToYUV444(matrix="Rec709", chromaresample="point")
ConvertToYUV420(chromaresample="lanczos")
ConvertBits(bits=8, dither=0)