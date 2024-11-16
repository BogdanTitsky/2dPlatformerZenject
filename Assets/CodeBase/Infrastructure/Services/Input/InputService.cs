using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const string AttackButton = "Fire1";
    private const string JumpButton = "Jump";
    private const string EscapeButton = "Cancel";

    public abstract Vector2 Axis { get; }

    public bool IsAttackButtonDown() => 
      SimpleInput.GetButtonDown(AttackButton);
    
    public bool IsJumpButtonDown() => 
      SimpleInput.GetButtonDown(JumpButton);
    public bool IsJumpButtonUp() => 
      SimpleInput.GetButtonUp(JumpButton);

    public bool IsEscapeButtonDown() =>
      SimpleInput.GetButtonDown(EscapeButton);

    protected static Vector2 SimpleInputAxis()
    {
      return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
  }
}