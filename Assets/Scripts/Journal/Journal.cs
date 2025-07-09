using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal : MonoBehaviour
{
    [SerializeField]
    float pageSpeed = 0.5f;

    [SerializeField]
    public List<Transform> pages;

    int index = -1;
    bool rotate = false;

    public GameObject prevButton;
    public GameObject nextButton;

    //We can't turn pages until we have created a second page, BuildJournal.cs enables the "next page" button once we have added a second.
    private void Start()
    {
        nextButton.SetActive(false);
        prevButton.SetActive(false);
    }

    //Set the initial state for out journal, by rotating all pages to 0 (in our case it will always be just one page)
    //This happens after taking our first photo. When app starts, journal has 0 pages, when a photo is taken, we add our first page. Check BuildJournal.cs
    public void InitialState()
    {
        for (int i = 0; i < pages.Count; i++)
        {
            pages[i].transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        pages[0].SetAsLastSibling();
    }

    //Set up the rotation to next page
    public void RotateNext()
    {
        //If we are already rotating a page, cancel
        if (rotate == true) { return; }
        index++;
        float angle = -180; //rotate page 180 degrees around center y axis

        //When we turn a page, will we be able to turn another or just turn back
        NextButtonActions();

        //Set active page as last sibling to make sure it renders in front of other pages
        pages[index].SetAsLastSibling();

        //Start the rotation
        StartCoroutine(Rotate(angle, true));
    }

    //If we are rotating to the next page, can we rotate again using our buttons?
    public void NextButtonActions()
    {
        //If we are rotating forward, it is assumed we can always go to the previous page
        if (prevButton.activeInHierarchy == false)
        {
            prevButton.SetActive(true);
        }

        //If we are at our page limit, then disable the "next page" button
        if (index == pages.Count - 1)
        {
            nextButton.SetActive(false);
        }
    }
    
    //Set up the rotation to the previous page
    public void RotatePrev()
    {
        //If we are already rotating a page, cancel
        if (rotate == true) { return; }
        float angle = 0; //angle after turning is -180, rotate back to 0 when turning back

        //Set active page as last sibling to make sure it renders in front of other pages
        pages[index].SetAsLastSibling();

        //When we go back a page, will we be able to go back another or just go to the next
        PrevButtonActions();

        //Start the rotation
        StartCoroutine(Rotate(angle, false));
    }

    //If we are rotating to the previous page, can we rotate again using our buttons?
    public void PrevButtonActions()
    {
        //If we are rotating back, it is assumed we can always go to the next page
        if (nextButton.activeInHierarchy == false)
        {
            nextButton.SetActive(true);
        }

        //If we are at our base index, then disable the "prev page" button
        if (index - 1 == -1)
        {
            prevButton.SetActive(false);
        }
    }

    //Action that rotates the page
    IEnumerator Rotate(float angle, bool forward)
    {
        float value = 0f;
        while (true)
        {
            //Rotate around the y axis at the specified angle and speed
            Quaternion targetRotation = Quaternion.Euler(pages[index].localRotation.x, angle, pages[index].localRotation.z);
            value += Time.deltaTime * pageSpeed;
            pages[index].localRotation = Quaternion.Slerp(pages[index].localRotation, targetRotation, value);

            //If we are within a small threshold of the page about to be done rotating, allow the next page to be rotated and decrease the page index if we went back a page, and break from the loop
            float angle1 = Quaternion.Angle(pages[index].localRotation, targetRotation);
            if (angle1 < 0.1f)
            {
                if (forward == false)
                {
                    index--;
                }
                rotate = false;
                break;
            }

            yield return null;
        }
    }
}
