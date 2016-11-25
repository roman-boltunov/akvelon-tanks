using JetBrains.Annotations;

using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// The tank movement.
/// </summary>
public class TankMovement : NetworkBehaviour
{
    /// <summary>
    /// The player number.
    /// Used to identify which tank belongs to which player.  This is set by this tank's manager.
    /// </summary>
    [SerializeField]
    public int playerNumber = 1;

    /// <summary>
    /// The local id.
    /// </summary>
    [SerializeField]
    public int localID = 1;

    /// <summary>
    /// The speed.
    /// </summary>
    [SerializeField]
    public float speed = 12f;                   // How fast the tank moves forward and back.

    /// <summary>
    /// The turn speed.
    /// </summary>
    [SerializeField]
    public float turnSpeed = 180f;              // How fast the tank turns in degrees per second.

    /// <summary>
    /// The pitch range.
    /// </summary>
    [SerializeField]
    public float pitchRange = 0.2f;             // The amount by which the pitch of the engine noises can vary.

    /// <summary>
    /// The movement audio.
    /// </summary>
    [SerializeField]
    public AudioSource movementAudio;           // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.

    /// <summary>
    /// The engine idling.
    /// </summary>
    [SerializeField]
    public AudioClip engineIdling;              // Audio to play when the tank isn't moving.

    /// <summary>
    /// The engine driving.
    /// </summary>
    [SerializeField]
    public AudioClip engineDriving;             // Audio to play when the tank is moving.

    /// <summary>
    /// The left dust trail.
    /// </summary>
    [SerializeField]
    public ParticleSystem leftDustTrail;        // The particle system of dust that is kicked up from the left track.

    /// <summary>
    /// The right dust trail.
    /// </summary>
    [SerializeField]
    public ParticleSystem rightDustTrail;       // The particle system of dust that is kicked up from the rightt track.

    /// <summary>
    /// The rigidbody.
    /// </summary>
    [SerializeField]
    public Rigidbody rigidbody;              // Reference used to move the tank.

    /// We freeze the rigibody when the control is disabled to avoid the tank drifting!
    /// <summary>
    /// The original constrains.
    /// </summary>
    protected RigidbodyConstraints originalConstrains;

    /// <summary>
    /// The movement axis.
    /// </summary>
    private string verticalAxis;              // The name of the input axis for moving forward and back.

    /// <summary>
    /// The turn axis.
    /// </summary>
    private string horizontalAxis;                  // The name of the input axis for turning.

    /// <summary>
    /// The movement input.
    /// </summary>
    private float verticalInput;              // The current value of the movement input.

    /// <summary>
    /// The turn input.
    /// </summary>
    private float horizontalInput;                  // The current value of the turn input.

    /// <summary>
    /// The original pitch.
    /// </summary>
    private float originalPitch;              // The pitch of the audio source at the start of the scene.

    /// <summary>
    /// The turret.
    /// </summary>
    private GameObject turret;

    private GameObject playerCamera;

    /// This function is called at the start of each round to make sure each tank is set up correctly.
    /// <summary>
    /// The set defaults.
    /// </summary>
    public void SetDefaults()
    {
        this.rigidbody.velocity = Vector3.zero;
        this.rigidbody.angularVelocity = Vector3.zero;

        this.verticalInput = 0f;
        this.horizontalInput = 0f;

        this.leftDustTrail.Clear();
        this.leftDustTrail.Stop();

        this.rightDustTrail.Clear();
        this.rightDustTrail.Stop();
    }

    /// <summary>
    /// The re enable particles.
    /// </summary>
    public void ReEnableParticles()
    {
        this.leftDustTrail.Play();
        this.rightDustTrail.Play();
    }

    /// <summary>
    /// The on start local player.
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        this.AttachCameraToTank();
        base.OnStartLocalPlayer();
    }

    /// <summary>
    /// The awake.
    /// </summary>
    [UsedImplicitly]
    private void Awake()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// The start.
    /// </summary>
    [UsedImplicitly]
    private void Start()
    {
        // The axes are based on player number.
        this.horizontalAxis = "Horizontal" + (this.localID + 1);
        this.verticalAxis = "Vertical" + (this.localID + 1);
        

        var renderers = this.transform.Find("TankRenderers");
        this.turret = renderers.transform.Find("TankTurret").gameObject;

        // Store the original pitch of the audio source.
        this.originalPitch = this.movementAudio.pitch;
    }

    /// <summary>
    /// The attach camera to tank.
    /// </summary>
    private void AttachCameraToTank()
    {
        var renderers = this.transform.Find("TankRenderers");
        this.turret = renderers.transform.Find("TankTurret").gameObject;
        this.playerCamera = GameObject.FindGameObjectWithTag("MainCamera");
        this.playerCamera.transform.parent = this.gameObject.transform;
        this.playerCamera.transform.localPosition = new Vector3(0, 2.2f, 0);
        this.playerCamera.transform.localRotation = Quaternion.identity;
        this.playerCamera.transform.forward = this.turret.transform.forward;
    }

    /// <summary>
    /// The update.
    /// </summary>
    [UsedImplicitly]
    private void Update()
    {
        if (!this.isLocalPlayer)
        {
            return;
        }

        // Store the value of both input axes.
        this.verticalInput = Input.GetAxis(this.verticalAxis);
        this.horizontalInput = Input.GetAxis(this.horizontalAxis);

        this.mova = 0.1f * transform.forward * this.speed * Time.deltaTime;
        this.rotat = 0.1f * this.turnSpeed * Time.deltaTime;

        this.turret.transform.forward = this.playerCamera.transform.forward;

        this.EngineAudio();
    }

    private float rotat;

    private Vector3 mova;

    /// <summary>
    /// The engine audio.
    /// </summary>
    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(this.verticalInput) < 0.1f && Mathf.Abs(this.horizontalInput) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (this.movementAudio.clip == this.engineDriving)
            {
                // ... change the clip to idling and play it.
                this.movementAudio.clip = this.engineIdling;
                this.movementAudio.pitch = Random.Range(this.originalPitch - this.pitchRange, this.originalPitch + this.pitchRange);
                this.movementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and the idling clip is currently playing...
            if (this.movementAudio.clip == this.engineIdling)
            {
                // ... change the clip to driving and playing.
                this.movementAudio.clip = this.engineDriving;
                this.movementAudio.pitch = Random.Range(this.originalPitch - this.pitchRange, this.originalPitch + this.pitchRange);
                this.movementAudio.Play();
            }
        }
    }

    /// <summary>
    /// The fixed update.
    /// </summary>
    [UsedImplicitly]
    private void FixedUpdate()
    {
        if (!this.isLocalPlayer)
        {
            return;
        }

        // Push left and right (move forward)
        if (this.verticalInput > 0 && this.horizontalInput > 0)
        {
            this.rigidbody.MovePosition(this.rigidbody.position + this.mova);
        }
        // Pull left and right (move backwards)
        else if (this.verticalInput < 0 && this.horizontalInput < 0)
        {
            this.rigidbody.MovePosition(this.rigidbody.position - this.mova);
        }
        // Push left and pull right (rotate to right)
        else if (this.verticalInput > 0 && this.horizontalInput < 0)
        {
            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, this.rotat, 0f));
        }
        // Push right and pull left (rotate to left)
        else if (this.verticalInput < 0 && this.horizontalInput > 0)
        {
            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, -this.rotat, 0f));
        }
        // Push left forward (move right forward)
        else if (this.verticalInput > 0)
        {
            this.rigidbody.MovePosition(this.transform.position + this.mova);

            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, this.rotat, 0f));
        }
        // Push right forward (move left forward)
        else if (this.horizontalInput > 0)
        {
            this.rigidbody.MovePosition(this.transform.position + this.mova);

            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, -this.rotat, 0f));
        }
        // Pull left (move right backwards)
        else if (this.verticalInput < 0)
        {
            this.rigidbody.MovePosition(this.transform.position - this.mova);

            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, this.rotat, 0f));
        }
        // Pull right forward (move left backwards)
        else if (this.horizontalInput < 0)
        {
            this.rigidbody.MovePosition(this.transform.position - this.mova);

            this.rigidbody.MoveRotation(this.rigidbody.rotation * Quaternion.Euler(0f, -this.rotat, 0f));
        }

        // Adjust the rigidbodies position and orientation in FixedUpdate.
        // this.Move();
        // this.Turn();
    }

    /// <summary>
    /// The move.
    /// </summary>
    private void Move()
    {
        // Create a movement vector based on the input, speed and the time between frames, in the direction the tank is facing.
        Vector3 movement = transform.forward * this.verticalInput * this.speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        this.rigidbody.MovePosition(this.rigidbody.position + movement);
    }

    /// <summary>
    /// The turn.
    /// </summary>
    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        var turn = this.horizontalInput * this.turnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        var inputRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        this.rigidbody.MoveRotation(this.rigidbody.rotation * inputRotation);
    }

    /// <summary>
    /// The on disable.
    /// </summary>
    [UsedImplicitly]
    private void OnDisable()
    {
        this.originalConstrains = this.rigidbody.constraints;
        this.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    /// <summary>
    /// The on enable.
    /// </summary>
    [UsedImplicitly]
    private void OnEnable()
    {
        this.rigidbody.constraints = this.originalConstrains;
    }
}