using UnityEngine;

public interface IPlayerMoveState
{
    /// <summary : State_Pattern>
    ///
    /// State machine for Player Movements
    /// 
    /// Machine delegates behavior through [PlayerContext.cs]
    /// 
    /// </summary>


    // SECTION - Method - State Specific -------------------------------------------------------------------
    public void OnLook(PlayerContext context);
    public void OnMove(PlayerContext context);
    public void OnCastMain(PlayerContext context);
    public void OnCastSecondary(PlayerContext context);
    public void OnCastSpace(PlayerContext context);


    // SECTION - Method - General -------------------------------------------------------------------
    public void OnStateEnter(PlayerContext context);
    public void OnStateUpdate(PlayerContext context);
    public IPlayerMoveState OnStateExit(PlayerContext context);
}
