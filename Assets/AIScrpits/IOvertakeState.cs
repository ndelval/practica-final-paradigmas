using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOvertakeState
{
    void Enter(OvertakeManager overtakeManager);
    void UpdateState(OvertakeManager overtakeManager);
    void ExitState(OvertakeManager overtakeManager);
}

public class NoOvertakingState : IOvertakeState
{
    public void Enter(OvertakeManager overtakeManager)
    {
        // No specific entry actions
    }

    public void UpdateState(OvertakeManager overtakeManager)
    {
        bool carAheadCenter, carAheadLeft, carAheadRight;
        overtakeManager.sensorManager.CheckFrontObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);

        bool rightSideClear, leftSideClear;
        overtakeManager.sensorManager.CheckSideClearance(out rightSideClear, out leftSideClear);

        if (carAheadCenter && rightSideClear)
        {
            overtakeManager.SetState(new OvertakingRightState());
        }
        else if (carAheadCenter && leftSideClear)
        {
            overtakeManager.SetState(new OvertakingLeftState());
        }
    }

    public void ExitState(OvertakeManager overtakeManager)
    {
        // No specific actions on exit
    }
}

public class OvertakingRightState : IOvertakeState
{
    public void Enter(OvertakeManager overtakeManager)
    {
        overtakeManager.StartOvertaking(1);
    }

    public void UpdateState(OvertakeManager overtakeManager)
    {
        bool carAheadCenter, carAheadLeft, carAheadRight;
        overtakeManager.sensorManager.CheckFrontObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);
        overtakeManager.ContinueOvertaking(!carAheadRight, !carAheadLeft);
    }

    public void ExitState(OvertakeManager overtakeManager)
    {
        overtakeManager.ResetOvertake();
    }
}

public class OvertakingLeftState : IOvertakeState
{
    public void Enter(OvertakeManager overtakeManager)
    {
        overtakeManager.StartOvertaking(-1);
    }

    public void UpdateState(OvertakeManager overtakeManager)
    {
        bool carAheadCenter, carAheadLeft, carAheadRight;
        overtakeManager.sensorManager.CheckFrontObstacle(out carAheadCenter, out carAheadLeft, out carAheadRight);
        overtakeManager.ContinueOvertaking(!carAheadRight, !carAheadLeft);
    }

    public void ExitState(OvertakeManager overtakeManager)
    {
        overtakeManager.ResetOvertake();
    }
}