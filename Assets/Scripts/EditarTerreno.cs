using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EditarTerreno : MonoBehaviour {

    public int planagemBuffuer = 2;
    public GameObject[] edificio;
    public Terrain terreno;
    public Material[] materials;
    public static int index=0;

    private GameObject[] edificioSombra;
    private TerrainData terrainData;
    private TerrainData oldTerrainData;
    private bool valid = false;
    private bool coliding = false;
    // Use this for initialization
    private void Start() {
        int totalEdificios = edificio.Length;
        edificioSombra = new GameObject[totalEdificios];
        terrainData = terreno.terrainData;
        oldTerrainData = terreno.terrainData;
        for(int i=0; i < totalEdificios; i++) {
            edificioSombra[i] = Instantiate(edificio[i], new Vector3(0, 0, 0), Quaternion.identity);
            edificioSombra[i].layer = 10;
        }
        //edificioSombra = Instantiate(edificio[index], new Vector3(0, 0, 0), Quaternion.identity);
        //edificioSombra.layer = 9;
        BuildingController.outColision += NaoEstaColidir;
        BuildingController.onColision += EstaColidir;
        BuildingController.eventoTeste += TestarEvento;
    }


    // Update is called once per frame
    void Update() {
        //mudar tudo para raycastAll
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        Vector3 angulo = new Vector3(0, 0, 0);
        //quando anda com o rato no terreno
        foreach (RaycastHit hit in hits) {
            if (hit.collider.tag == "Ground") {
                valid = VerificarEspaco(hit);
                if (valid&&coliding)
                    edificioSombra[index].GetComponentInChildren<Renderer>().material = materials[0];
                else
                    edificioSombra[index].GetComponentInChildren<Renderer>().material = materials[1];
                edificioSombra[index].transform.position = hit.point;
                break;
            }
        }

        //quando clica no terreno
        //está a alizar o terreno um bocado acima do que devia
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("rato primido");
            foreach (RaycastHit hit in hits) {
                Debug.DrawLine(hit.transform.position, Vector3.up * 5);
                if (hit.collider.tag == "Ground" && valid && coliding) {
                    FazerPlanagem(hit);
                    Instantiate(edificio[index], hit.point, Quaternion.Euler(angulo));
                    print("It's working");
                }
            }
        }
    }
    
    public void ResetTerrain() {
        int xRes = oldTerrainData.heightmapWidth;
        int yRes = oldTerrainData.heightmapHeight;
        float[,] heights = oldTerrainData.GetHeights(0, 0, xRes, yRes);
        oldTerrainData.SetHeights(0, 0, heights);
        foreach (GameObject edificio in GameObject.FindGameObjectsWithTag("Edificio")) {
            Destroy(edificio);
        }
    }

    private void NaoEstaColidir(Collider collision) {
        coliding = true;
        Debug.Log("A Luciana  Nao Está Gravida: Está a colidir");
    }

    private void EstaColidir(Collider collision) {
        coliding = false;
        Debug.Log("A Luciana Está Gravida: Está a colidir");
    }

    private void TestarEvento() {
        Debug.Log("A Luciana Está Gravida");
    }

    private bool VerificarEspaco(RaycastHit hitInfo) {
        float maxHeight = 0, minHeight = 1;
        int xRes = terrainData.heightmapWidth;
        int yRes = terrainData.heightmapHeight;
        float[,] heights = terrainData.GetHeights(0, 0, xRes, yRes);
        //calcula a posiçao que o edificio vai ficar no terreno
        Vector2 posEdificio = new Vector2();
        Vector3 terrenoPos = terreno.transform.position;
        Vector3 tamanhoEdificio = edificio[index].GetComponentInChildren<Renderer>().bounds.size;
        Vector3 tamanhoNoTerreno;
        posEdificio.y = hitInfo.point.x - terrenoPos.x;
        posEdificio.x = hitInfo.point.z - terrenoPos.z;
        posEdificio.x *= xRes / terrainData.size.z;
        posEdificio.y *= yRes / terrainData.size.x;
        tamanhoNoTerreno.y = tamanhoEdificio.x * xRes / terrainData.size.z;
        tamanhoNoTerreno.x = tamanhoEdificio.z * yRes / terrainData.size.x;
        //mudifica as height do terreno para criar a planagem
        //tenho de descobrir a altura que o terreno vai ficar
        //x = edificio.tamanho (depois no loop) i = posEdificio.x - (x / 2); i < = posEdificio.x + (x / 2)
        //if ()
        for (int i = (int)-(tamanhoNoTerreno.x / 2) + (int)posEdificio.x - planagemBuffuer; i < (int)(tamanhoNoTerreno.x / 2) + (int)posEdificio.x + planagemBuffuer; i++) {
            for (int j = (int)-tamanhoNoTerreno.y / 2 + (int)posEdificio.y - planagemBuffuer; j < (int)tamanhoNoTerreno.y / 2 + (int)posEdificio.y + planagemBuffuer; j++) {
                if (heights[i, j] > maxHeight)
                    maxHeight = heights[i, j];
                if (heights[i, j] < minHeight)
                    minHeight = heights[i, j];
            }
        }
        //heights[(int) posEdificio.x, (int) posEdificio.y] = .5f;
        if (maxHeight - minHeight <= 0.017) {
            return true;
        }
        Debug.Log("max-min: " + (maxHeight - minHeight));
        return false;
    }

    private void FazerPlanagem(RaycastHit hitInfo) {
        int xRes = terrainData.heightmapWidth;
        int yRes = terrainData.heightmapHeight;
        float[,] heights = terrainData.GetHeights(0, 0, xRes, yRes);
        //calcula a posiçao que o edificio vai ficar no terreno
        Vector2 posEdificio = new Vector2();
        Vector3 terrenoPos = terreno.transform.position;
        Vector3 tamanhoEdificio = edificio[index].GetComponentInChildren<Renderer>().bounds.size;
        Vector3 tamanhoNoTerreno;
        posEdificio.y = hitInfo.point.x - terrenoPos.x;
        posEdificio.x = hitInfo.point.z - terrenoPos.z;
        posEdificio.x *= xRes / terrainData.size.z;
        posEdificio.y *= yRes / terrainData.size.x;
        tamanhoNoTerreno.y = tamanhoEdificio.x * xRes / terrainData.size.z;
        tamanhoNoTerreno.x = tamanhoEdificio.z * yRes / terrainData.size.x;
        //mudifica as height do terreno para criar a planagem
        //tenho de descobrir a altura que o terreno vai ficar
        //x = edificio.tamanho (depois no loop) i = posEdificio.x - (x / 2); i < = posEdificio.x + (x / 2)
        for (int i = (int)-(tamanhoNoTerreno.x / 2) + (int)posEdificio.x - planagemBuffuer; i < (int)(tamanhoNoTerreno.x / 2) + (int)posEdificio.x + planagemBuffuer; i++) {
            for (int j = (int)-tamanhoNoTerreno.y / 2 + (int)posEdificio.y - planagemBuffuer; j < (int)tamanhoNoTerreno.y / 2 + (int)posEdificio.y + planagemBuffuer; j++) {
                heights[i, j] = hitInfo.point.y / 300;
            }
        }
        //heights[(int) posEdificio.x, (int) posEdificio.y] = .5f;
        Debug.Log("altura" + heights[(int)posEdificio.x, (int)posEdificio.y] + "posicao x: " + (int)posEdificio.x + "posicao y: " + (int)posEdificio.y + " " + xRes + " " + yRes + " " + terrainData.alphamapHeight + " " + terrainData.detailHeight);

        terrainData.SetHeights(0, 0, heights);
    }
}
