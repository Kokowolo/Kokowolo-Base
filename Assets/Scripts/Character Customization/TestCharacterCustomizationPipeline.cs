/*
 * File Name: TestCharacterCustomizationPipeline.cs
 * Description: This script is for ...
 * 
 * Author(s): Kokowolo, Will Lacey
 * Date Created: March 22, 2023
 * 
 * Additional Comments:
 *		File Line Length: 120
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Kokowolo.CharacterCustomization;

public class TestCharacterCustomizationPipeline : MonoBehaviour
{
    /************************************************************/
    #region Fields

    [SerializeField] Customization prefab;

    #endregion
	/************************************************************/
    #region Properties

    #endregion
    /************************************************************/
    #region Functions

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CreateCharacter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ReleaseCharacter();
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Customization customization = FindObjectOfType<Customization>();
        //     // ApplyCustomizerToCustomization(customization);
        // }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Customizer.Instance.SetToRandomCharacter();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Customizer.Instance.SwitchCharacterGender();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Pool.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Pool.LoadRandom();
        }
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     Debug.Log(male);
        //     Debug.Log(female);
        // }
    }

    private void CreateCharacter()
    {
        Customization character = Instantiate(prefab);
        Customizer.Instance.LoadCustomization(character);
        Customizer.Instance.SetToRandomCharacter();

        character.transform.position = new Vector3(0.4f, 0, 0);
        character.transform.rotation = Quaternion.Euler(new Vector3(0, 210, 0));
    }

    private void ReleaseCharacter()
    {
        Customizer.Instance.LoadDefaultCustomization();
    }
    
    #endregion
    /************************************************************/
}