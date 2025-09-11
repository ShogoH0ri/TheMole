using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotor : MonoBehaviour
{
    [Header("MovementSetting")]
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 2f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    public bool sprinting;
    public bool crouching;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public bool lerpCrouch;
    public float crouchTimer;
    public int killcount= 0;

    public int RequiredKills;

    [Header("KeySetting")]
    public bool hasKey = false;
    public GameObject keyPrefab;
    public Transform keySpawn;
    public Transform keyHolder;

    [SerializeField]
    private GameObject door;

    [Header ("WeaponSetting")]
    public Camera playerCamera;
    public bool hasWeapon = false;
    public GameObject weaponPrefab;
    public Transform weaponHolder;
    public GameObject currentWeapon;
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public bool IsCrouching => crouching;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Fall();

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
                
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        if (hasWeapon && Mouse.current.leftButton.wasPressedThisFrame)
        {
            FireWeapon();
        }
    }

    public void Fall()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded )
        {
            if(playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }
        }
        else
        {
            if(playerVelocity.y > 0)
            {
                playerVelocity.y += gravity * lowJumpMultiplier * Time.deltaTime;
            }
            else if (playerVelocity.y < 0)
            {
                playerVelocity.y += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                playerVelocity.y += gravity * Time.deltaTime;
            }
        
        }
    }
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        float currentSpeed;
        if (crouching)
        {
            currentSpeed = crouchSpeed;
        }
        else if (sprinting)
        {
            currentSpeed = sprintSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            sprinting = true;
        }
        else if (context.canceled)
        {
            sprinting = false;
        }
    }
    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void EquipKey()
    {
        hasKey = true;
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }
        currentWeapon = Instantiate(keyPrefab, keyHolder.position, keyHolder.rotation, keyHolder);
        hasWeapon = false;
    }

    public void EquipWeapon()
    {
        hasWeapon = true;
        currentWeapon = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation,weaponHolder);
    }

    public void FireWeapon()
    {
        if (!hasWeapon) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, playerCamera.transform.rotation);
        SoundManager.Instance.PlaySound3D("Shooting", transform.position);

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = playerCamera.transform.forward * bulletVelocity;
            rb.AddForce(playerCamera.transform.forward * bulletVelocity, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));
    }

    public void KillCount()
    {
        killcount++;
        if (killcount >= RequiredKills && door != null)
        {
            GameObject key = Instantiate(keyPrefab, keySpawn.position, Quaternion.Euler(0f, 90f, 180f));
        }
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
