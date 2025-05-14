3D MODELS
This package contains two meshes:
- The body (4044 tris),
- The wings (8 tris).

The parts (core, legs, eyes and antennas) of the body are separated objects so you can disable them if you wish.

PREFABS
"Butterfly_Animated" is the main prefab, any change on this prefab will be applied on all the butterflies. Each butterfly has a prefab with their own wings materials/textures.

MATERIALS/TEXTURES
There are 146 materials for the wings, each material has its own texture file (1024x1024).
The body has 1 material, the eyes have their own material using a normal texture file (1024x1024)

There are 16 types of wings and each has variants/ different colours.

BUTTERFLY ANIMATION
There are five animations:
- Start flying
- Fly (loop)
- Stop flying
- IDLE 1 (loop)
- IDLE 2

SCRIPTS
The butterfly animation speed can be changed using "ChangeSpeedAnimation.cs" (require Animator on the object):
- To give a specific speed, disable "Randomized Speed" and update "Given speed" value
- To have a random speed, enable "Randomized Speed" and update "minimum Speed" and "Maximum Speed" value

SHADER
"Cutout_TwoSide" : Show wings on both side of the mesh with cutout. (Only for built-in as the URP and HDRP has their own shader for this purpose.)

EXAMPLE SCENES
Two scenes are available:
- "All Butterflies" shows all the butterflies
- "Butterfly flies" shows a butterfly flying from point A to point B (using Timeline).