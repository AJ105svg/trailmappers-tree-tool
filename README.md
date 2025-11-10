# trailmappers-tree-tool
Very not user friendly tool to generate large amounts of trees using heatmaps for trailmappers projects

## how to use
import and spawn in your terrain mesh, size accordingly. make sure you add on a mesh collider for the terrain (coordinate scale should be the same as trailmappers)
import your heatmap as a texture2d. make sure you enable read/write in the asset settings otherwise it wont work

set up inspector settings in the generator gameobject
run the game, and it will save the a map.json file into the assets folder of the trees generated, which you can place in your Documents\Trailmappers\Saves folder

the prefab and offsets used are hardcoded so enjoy
