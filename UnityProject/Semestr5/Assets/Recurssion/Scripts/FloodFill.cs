using UnityEngine;
using UnityEngine.Tilemaps;

namespace Recurssion.Assets.Scripts.Recurssion.Scripts
{
    public class FloodFill : MonoBehaviour
    {

        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileBase coloredTile;
        [SerializeField] private TileBase originalTile;


        void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int tilePosition = tilemap.WorldToCell(mouseWorldPos);
                StartFloodFill(tilePosition);
            }
        }

        public void StartFloodFill(Vector3Int position)
        {
            originalTile = tilemap.GetTile(position); // Set the origin tile (Where to start)
            if (originalTile != coloredTile)
                Fill(position);
        }

        void Fill(Vector3Int pos)
        {
            if (tilemap.GetTile(pos) != originalTile) return; // The base case (aka - תנאי עצירה)

            tilemap.SetTile(pos, coloredTile); // Color the tile

            Fill(pos + Vector3Int.up);   
            Fill(pos + Vector3Int.down);  
            Fill(pos + Vector3Int.left);  
            Fill(pos + Vector3Int.right); 
        }
    }
}