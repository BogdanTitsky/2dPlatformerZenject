using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService : IService
  {
    Vector2 Axis { get; }

    bool IsAttackButtonDown();
    bool IsJumpButtonDown();
    bool IsJumpButtonUp();
    bool IsEscapeButtonDown();
    bool IsBlockButtonDown();
    bool IsBlockButtonUp();
  }
}