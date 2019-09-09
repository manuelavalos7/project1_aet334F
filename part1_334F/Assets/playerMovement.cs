using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour
{
    private int health= 100; //The Amount of health the player has
    private float speed = 10f;//The speed of the player
    private bool isGrounded = true;// if true, then the character is on the ground and can start jumping again
    private int jumps = 3;//The player can jump three times in the air
    private bool hasKey = false;//Determines if the player has obtained the key
    public Text HealthText;// text object to display the player health
    public Text CoinText;// text object to display the player health
    public AudioSource damageSound;// sound that plays when player takes damage
    public AudioSource happySound;// sound that plays when key or chest is obtained
    public AudioSource coinSound;// sound for obtaining coins
    private int coins = 0;// ammount of coins the player has


    // Update is called once per frame
    void Update()
    {

        if (health <= 0 || transform.position.y < -5){//   if health is less than 0 or character position is less than -5 (in the water)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);// Reload the current scene
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {//If upArrow or W is pressed
            transform.position += Time.deltaTime * speed * Vector3.forward;// Increment character position forward using the characters speed               
            GetComponent<Animator>().SetTrigger("walk"); // Start walking Animation
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){// If RightArrow or D is pressed
            transform.position += Time.deltaTime * speed * Vector3.right;// Increment character position to the right
            GetComponent<Animator>().SetTrigger("walk");// Start walking Animation
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){// If LeftArrow or A is pressed
            transform.position += Time.deltaTime * speed * Vector3.left;// Increment character position left
            GetComponent<Animator>().SetTrigger("walk");// Start walking Animation
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){//If DownArrow or S pressed
            transform.position += Time.deltaTime * speed * Vector3.back;//Increment character position back
            GetComponent<Animator>().SetTrigger("walk");//Start walking Animation
        }
        if (Input.GetKeyDown(KeyCode.Space) && (jumps >0)) { //if Jump is pressed once and character has jumps left
            GetComponent<Animator>().SetTrigger("jump");// Start jump animation
            GetComponent<Rigidbody>().AddForce(Vector3.up * (150 / 3) * jumps);// Add force in upwards direction (divided by 3 and multiplied by numbr of jumps to make jumps lose strength with each jump)
            jumps--;// Player has one less jump now that they jumped
            isGrounded = false;// character is no longer on the ground
        }
    }

    private void OnCollisionEnter(Collision collision)// is called when character collides with an object
    {
        Debug.Log("Collided with: " + collision.other.tag);
        if (collision.other.tag == "Ground") {// if character hits the ground
            isGrounded = true;// character is grounded
            jumps = 3;// reset the character to 3 jumps because they are grounded
        }
        if (collision.other.tag == "Gate" && hasKey) {// if chracter touches gate and they have obtained the key
            collision.other.gameObject.GetComponent<Animator>().SetTrigger("openGate");//set open gate animation to start

        }
    }
    private void OnTriggerEnter(Collider other)//is called when character enters an object that is set as a trigger
    {
        if (other.tag == "Coin") {// if character touches coin
            coinSound.Play();// play coin sound
            coins++;// add 1 to the players coin count
            Instantiate(Resources.Load("coinParticles"), other.transform.position, other.transform.rotation); // play coin particle effect from resources with the same position and rotation as the coin
            CoinText.text = "Coins: " + coins;// change coin text to show updated number of coins
            Destroy(other.gameObject);// destroy the coin
        }
        if (other.tag == "Key") {// if character touches key (chest is also tagged as key for same effect)
            hasKey = true; // Character has key so they can open gate
            Instantiate(Resources.Load("particleEffect"), transform.position, transform.rotation);// play particle effect from resources folder
            Destroy(other.gameObject);//destroy the key
            happySound.Play();// play chicken clucking sound
        }
        if (other.tag == "Trap") {// if character enters trap trigger
            other.GetComponent<Animator>().SetTrigger("activateTrap");// play trap animation
            GetComponent<Animator>().SetTrigger("damage");// play chicken hurt animation
            health -= 10;// decrease player's health
            HealthText.text = "Health: " + health;// update health text to show updated health 
            damageSound.Play(); //play chicken damage sound
        }
    }
}
