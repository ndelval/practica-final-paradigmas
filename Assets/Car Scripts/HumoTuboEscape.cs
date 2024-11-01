using UnityEngine;

public class HumoTuboEscape : MonoBehaviour
{
    public ParticleSystem leftExhaustParticles;
    public ParticleSystem rightExhaustParticles;
    public CarController carController;
    private CarSetup carSetup;

    void Start()
    {
        carSetup = carController.GetComponent<CarSetup>();
        SetExhaustSmoke(carSetup.normalSmokeColor, carSetup.normalSmokeSpeed, carSetup.normalEmissionRate);
    }

    void Update()
    {
        bool shouldPlayParticles = carController.speed > 1f;

        if (carController.nitrusFlag && carController.nitrusValue > 0)
        {
            SetExhaustSmoke(carSetup.nitroSmokeColor, carSetup.nitroSmokeSpeed, carSetup.nitroEmissionRate);
        }
        else
        {
            SetExhaustSmoke(carSetup.normalSmokeColor, carSetup.normalSmokeSpeed, carSetup.normalEmissionRate);
        }

        if (shouldPlayParticles && !leftExhaustParticles.isPlaying)
        {
            leftExhaustParticles.Play();
            rightExhaustParticles.Play();
        }
        else if (!shouldPlayParticles && leftExhaustParticles.isPlaying)
        {
            leftExhaustParticles.Stop();
            rightExhaustParticles.Stop();
        }
    }

    private void SetExhaustSmoke(Color color, float speed, float rateOverTime)
    {
        var main1 = leftExhaustParticles.main;
        main1.startColor = color;
        main1.startSpeed = speed;

        var main2 = rightExhaustParticles.main;
        main2.startColor = color;
        main2.startSpeed = speed;

        var emission1 = leftExhaustParticles.emission;
        emission1.rateOverTime = rateOverTime;

        var emission2 = rightExhaustParticles.emission;
        emission2.rateOverTime = rateOverTime;
    }
}
