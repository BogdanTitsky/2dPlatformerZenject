using CodeBase.Data;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    [SerializeField] private float _movementSpeed;

    private IInputService _inputService;
    private Camera _camera;

    private void Awake()
    {
      _inputService = AllServices.Container.Single<IInputService>();
    }

    private void Start() =>
      _camera = Camera.main;

    private void Update()
    {
      Vector3 movementVector = Vector3.zero;

      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.z = 0;
        movementVector.y = 0;
        movementVector.Normalize();
      }
      transform.position += _movementSpeed * movementVector * Time.deltaTime;
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      if (savedPosition != null)
        transform.position = savedPosition.AsUnityVector();
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;

    
  }
}