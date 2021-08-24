#Global Variables
global targetWidth = 2880
global targetHeight = 2160

#Load Plugins and/or Scripts
LoadPlugin("nnedi3.dll")

AVISource("sample.avi")
Crop(17, 0, -13, 0)
Upscale()

function Upscale(Clip c)
{
    w = targetWidth/c.Width
	h = targetHeight/c.Height
	
	x = w > h ? w : h
	f = x <= 2 ? 2 : (x <= 4 ? 4 : (x <= 8 ? 8 : (x <= 16 ? 16 : (x <= 32 ? 32 : 64))))
	
	return c.nnedi3_rpow2(rfactor=f, nsize=0, nns=3, qual=1, cshift="lanczosresize", ep0=2, pscrn=2, fwidth=targetWidth, fheight=targetHeight)
}