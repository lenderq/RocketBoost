using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;

    [SerializeField] float thrustStrength = 10f;
    [SerializeField] float rotationSpeed = 10f;

    [SerializeField] ParticleSystem rocketJetSystem;
    [SerializeField] ParticleSystem rocketLeftJetSystem;
    [SerializeField] ParticleSystem rocketRightJetSystem;

    [SerializeField] AudioClip engineSound;
    [SerializeField] AudioClip sideEnginesSound;

    [SerializeField] AudioSource audioSourceSideJets;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotationRight();
        }
        else if (rotationInput > 0)
        {
            RotationLeft();
        }
        else
        {
            StopRotation();
        }

        PlaySideJetsSound();

        ApplyRotation(rotationInput);
    }

    private void RotationRight()
    {
        rocketLeftJetSystem.Stop();
        rocketRightJetSystem.Play();
    }

    private void RotationLeft()
    {
        rocketRightJetSystem.Stop();
        rocketLeftJetSystem.Play();
    }

    private void StopRotation()
    {
        rocketLeftJetSystem.Stop();
        rocketRightJetSystem.Stop();
    }

    private void PlaySideJetsSound()
    {
        if (this.rotation.IsPressed() && !audioSourceSideJets.isPlaying)
            audioSourceSideJets.PlayOneShot(sideEnginesSound);
        else if (!this.rotation.IsPressed() && audioSourceSideJets.isPlaying)
            audioSourceSideJets.Stop();
    }

    private void ApplyRotation(float rotation)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.back * rotationSpeed * Time.fixedDeltaTime * rotation);
        rb.freezeRotation = false;
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineSound);
        }
        if (!rocketJetSystem.isPlaying)
            rocketJetSystem.Play();
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        rocketJetSystem.Stop();
    }
}
