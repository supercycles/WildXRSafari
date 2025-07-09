using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Used for inputting taken photos into the "Journal" menu
public class BuildJournal : MonoBehaviour
{
    public List<Transform> pages;
    public int numPolaroids = -1;
    public int currNumPolosOnPage = 0;
    public int pageNum = 0;

    public GameObject pagePrefab;
    public Transform pagesParent;

    //Materials that prevent photos from showing on both sides of a page(image)
    public Material pageFrontMat;
    public Material pageFrontBehindPictureMat;

    //May be used in the future if both left and right page of journal is utilized
    //public Material pageBackMat;
    //public Material pageBackBehindPictureMat;

    //Called after we have taken a picture and displayed the sample polaroid on the screen
    public void AddPolaroid(GameObject front, GameObject back)
    {
        //Increment num polaroids
        numPolaroids++;

        //If our number can be cleanly divided by 4, it needs a new page (0, 4, 8, 12...)
        if (numPolaroids % 4 == 0)
        {
            pageNum++;

            //Instantiate page
            GameObject newPage = Instantiate(pagePrefab, pagesParent);
            pages.Add(newPage.transform);
            newPage.name = "Page" + pageNum.ToString();
            newPage.transform.GetChild(0).gameObject.name = "PageSprite" + pageNum.ToString();

            //Tell our journal it can build its behaviors now that it has a page
            transform.GetChild(1).GetComponent<Journal>().pages.Add(newPage.transform);
            newPage.transform.SetAsFirstSibling();
            if (pageNum == 1) { transform.GetChild(1).GetComponent<Journal>().InitialState(); }

            //If it is our second page, enable the next page button, Journal behavior will handle the rest
            else if (pageNum == 2) { transform.GetChild(1).GetComponent<Journal>().nextButton.SetActive(true); }
        }

        //Set the proper materials and inital positions of photos
        front.GetComponent<Image>().material = pageFrontMat;
        back.GetComponent<Image>().material = pageFrontBehindPictureMat;

        //Set parent to proper page
        front.transform.SetParent(pages[pageNum - 1].GetChild(0));
        back.transform.SetParent(pages[pageNum - 1].GetChild(0));

        //Set front (actual photo) to be last sibling so it always displays on top of the polaroid background
        front.transform.SetAsLastSibling();

        //Set init pos + rot
        front.transform.localPosition = front.transform.parent.localPosition;
        back.transform.localPosition = back.transform.parent.localPosition;
        front.transform.localRotation = Quaternion.Euler(0, 0, 0);
        back.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //Determine which polaroid we are putting on the page (0=NW, 1=NE, 2=SW, 3=SE)
        switch (currNumPolosOnPage)
        {
            case 0:
                currNumPolosOnPage++;
                front.transform.localPosition = new Vector3(-.08f, .11f, 0);
                back.transform.localPosition = new Vector3(-.08f, .09f, 0);
                break;
            case 1:
                currNumPolosOnPage++;
                front.transform.localPosition = new Vector3(.08f, .11f, 0);
                back.transform.localPosition = new Vector3(.08f, .09f, 0);
                break;
            case 2:
                currNumPolosOnPage++;
                front.transform.localPosition = new Vector3(-.08f, -.07f, 0);
                back.transform.localPosition = new Vector3(-.08f, -.09f, 0);
                break;
            case 3:
                front.transform.localPosition = new Vector3(.08f, -.07f, 0);
                back.transform.localPosition = new Vector3(.08f, -.09f, 0);
                currNumPolosOnPage = 0;
                break;
        }
    }
}
