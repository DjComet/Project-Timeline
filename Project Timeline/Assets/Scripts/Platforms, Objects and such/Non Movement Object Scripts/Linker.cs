using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Linker : MonoBehaviour {
    //A
    //Este script existe para que cada objeto lógico que envía señales de activación (botones, palancas..) lo tenga, como componente común, independientemente del script que sigan para activarse/moverse,
    //de esta forma es mucho más sencillo referenciarlos desde los objetos a activar ya que solo hay que buscar este script en los objetos palanca/botón y no hay que comprobar el booleano 'active' en el script de movimiento
    //del botón, o de la palanca, o del objeto que sea, solo en este script común.
    public ITimeAction timeAction;
    private TimeManagerScript timeManager;
    public bool activate = false;
    public bool active = false;

	// Use this for initialization
	void Start () {

        timeManager = TimeManagerScript.timeManager;
        timeAction = GetComponent<ITimeAction>();
	}
	
	// Update is called once per frame
	void Update () {
        
    }
    

}
