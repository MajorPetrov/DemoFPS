using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Crouch : MonoBehaviour
{
    private Transform _fpsControllerTransform;
    private CharacterController _characterController;
    private FirstPersonController _firstPersonController;

    public bool IsCrouching { get; set; }

    // Use this for initialization
    void Start()
    {
        // Oui je sais, c'est dégueulasse, mais je déteste mélanger les prefabs avec mes trucs personnels.
        _fpsControllerTransform = transform.parent.transform.parent;
        _characterController = _fpsControllerTransform.gameObject.GetComponent<CharacterController>();
        _firstPersonController = _fpsControllerTransform.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Crouch"))
            Crouching();
    }

    public void Crouching()
    {
        /*
         * J'ai enfin trouvé la technique! il s'agit de changer manuellement la position du FPSController,
         * Puis de changer sa taille. Un seul défaut à cette technique c'est la transition. Il n'y en a pas.
         * Peut-être est-il possible d'allouer une transition à l'animator de l'arme elle-même.
         * C'est à dire déclencher l'animation au moment de passer du stade debout au stade accroupis.
         * Et inversement bien entendu.
         */
        if (!IsCrouching)
        {
            IsCrouching = true;
            _firstPersonController.WalkSpeed = 2.0f;
            _fpsControllerTransform.position = new Vector3(_fpsControllerTransform.position.x,
                                                         _fpsControllerTransform.position.y - 0.399995f,
                                                           _fpsControllerTransform.position.z);
            _characterController.height = 1.0f;
        }
        else
        {
            IsCrouching = false;
            _firstPersonController.WalkSpeed = 5.0f;
            _fpsControllerTransform.position = new Vector3(_fpsControllerTransform.position.x,
                                                         _fpsControllerTransform.position.y + 0.399995f,
                                                           _fpsControllerTransform.position.z);
            _characterController.height = 1.8f;
        }
    }
}
