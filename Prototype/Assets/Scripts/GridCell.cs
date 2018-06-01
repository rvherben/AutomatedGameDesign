using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour {

    public string cellName;

    public Transform blockadesHolder;
    public Transform obstaclesHolder;
    public Transform resourcesHolder;

    Text _buttonText;
    List<Image> _backgroundImages;

    List<GameObject> _blockades;
    List<GameObject> _obstacles;
    List<GameObject> _resources;

    float cellHeat;

	public void Init (float heat)
    {
        _backgroundImages = new List<Image>();
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).name == "Background")
            {
                _backgroundImages.Add(transform.GetChild(i).GetComponent<Image>());
            }
        }
        _buttonText = transform.Find("ButtonText").GetComponent<Text>();

        cellHeat = heat;

        _blockades = new List<GameObject>();
        _obstacles = new List<GameObject>();
        _resources = new List<GameObject>();

        for(int i = 0; i < blockadesHolder.childCount; i++)
        {
            if(blockadesHolder.GetChild(i).name == cellName)
            {
                _blockades.Add(blockadesHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < obstaclesHolder.childCount; i++)
        {
            if (obstaclesHolder.GetChild(i).name == cellName)
            {
                _obstacles.Add(obstaclesHolder.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < resourcesHolder.childCount; i++)
        {
            if (resourcesHolder.GetChild(i).name == cellName)
            {
                _resources.Add(resourcesHolder.GetChild(i).gameObject);
            }
        }

        foreach (GameObject obj in _obstacles)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in _blockades)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in _resources)
        {
            obj.SetActive(false);
        }

    }

    public void UpdateCell()
    {
        //Activating obstacles based on how much heat there is
        foreach(GameObject obstacle in _obstacles)
        {
            obstacle.SetActive(false);
            obstacle.SetActive(Random.Range(0.0f, 100f) < Heat);
        }

        //Activating blockades based on how much heat there is
        foreach (GameObject blockade in _blockades)
        {
            blockade.SetActive(false);
            blockade.SetActive(Random.Range(0.0f, 100f) < Heat);
        }

        //Making sure that not all blockades in a cell are active at the same time
        if (_blockades.Count > 1)
        {
            int activeBlockadeCount=0;
            foreach(GameObject blockade in _blockades)
            {
                if(blockade.activeSelf == true)
                {
                    activeBlockadeCount++;
                }
            }
            if(activeBlockadeCount == _blockades.Count)
            {
                _blockades[Random.Range(0, _blockades.Count)].SetActive(false);
            }
        }

        //Deactivating resources based on how much heat there is
        foreach (GameObject resource in _resources)
        {
            resource.SetActive(false);
            resource.SetActive(Random.Range(0.0f, 100f) > Heat);
        }

        _buttonText.text = Mathf.Round(cellHeat).ToString();
    }

    //Sets cell background and heat text to visible or invisible based on passed boolean
    public void SetDiagnosticsActive(bool active)
    {
        _buttonText.gameObject.SetActive(active);
        if (active)
        {
            foreach(Image image in _backgroundImages)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.08f);
            }
        }
        else
        {
            foreach (Image image in _backgroundImages)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
            }
        }
    }

    public float Heat
    {
        get
        {
            return cellHeat;
        }
        set
        {
            cellHeat = value;
        }
    }


}
