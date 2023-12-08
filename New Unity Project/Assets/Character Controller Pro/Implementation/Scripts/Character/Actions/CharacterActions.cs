using Lightbug.CharacterControllerPro.Implementation;

[System.Serializable]
public struct CharacterActions
{

    // Bool actions
    public BoolAction @jump;
    public BoolAction @run;
    public BoolAction @interact;
    public BoolAction @jetPack;
    public BoolAction @dash;
    public BoolAction @crouch;
    public BoolAction @attack;
    public BoolAction @grap;
    public BoolAction @test;
    public BoolAction @spAttack;
    public BoolAction @defend;

    // Float actions
    public FloatAction @pitch;
    public FloatAction @roll;

    // Vector2 actions
    public Vector2Action @movement;

    /// <summary>
    /// Reset all the actions.
    /// </summary>
    public void Reset()
    {
        @jump.Reset();
        @run.Reset();
        @interact.Reset();
        @jetPack.Reset();
        @dash.Reset();
        @crouch.Reset();
        @attack.Reset();
        @grap.Reset();
        @test.Reset();
        @spAttack.Reset();
        @defend.Reset();


        @pitch.Reset();
        @roll.Reset();

        @movement.Reset();
    }

    /// <summary>
    /// Initializes all the actions by instantiating them. Each action will be instantiated with its specific type (Bool, Float, or Vector2).
    /// </summary>
    public void InitializeActions()
    {
        @jump = new BoolAction();
        @jump.Initialize();

        @run = new BoolAction();
        @run.Initialize();

        @interact = new BoolAction();
        @interact.Initialize();

        @jetPack = new BoolAction();
        @jetPack.Initialize();

        @dash = new BoolAction();
        @dash.Initialize();

        @crouch = new BoolAction();
        @crouch.Initialize();

        @attack = new BoolAction();
        @attack.Initialize();

        @defend = new BoolAction();
        @defend.Initialize();

        @grap = new BoolAction();
        @grap.Initialize();

        @test = new BoolAction();
        @test.Initialize();

        @spAttack = new BoolAction();
        @spAttack.Initialize();

        @pitch = new FloatAction();
        @roll = new FloatAction();

        @movement = new Vector2Action();
    }

    /// <summary>
    /// Updates the values of all the actions based on the current input handler (human).
    /// </summary>
    public void SetValues(InputHandler inputHandler)
    {
        if (inputHandler == null)
            return;

        @jump.value = inputHandler.GetBool("Jump");
        @run.value = inputHandler.GetBool("Run");
        @interact.value = inputHandler.GetBool("Interact");
        @jetPack.value = inputHandler.GetBool("Jet Pack");
        @dash.value = inputHandler.GetBool("Dash");
        @crouch.value = inputHandler.GetBool("Crouch");
        @attack.value = inputHandler.GetBool("attack");
        @grap.value = inputHandler.GetBool("grap");
        @spAttack.value = inputHandler.GetBool("spAttack");
        @test.value = inputHandler.GetBool("Test");//Ì§Æð
        @defend.value = inputHandler.GetBool("Defend");

        @pitch.value = inputHandler.GetFloat("Pitch");
        @roll.value = inputHandler.GetFloat("Roll");

        @movement.value = inputHandler.GetVector2("Movement");
    }

    /// <summary>
    /// Copies the values of all the actions from an existing set of actions.
    /// </summary>
    public void SetValues(CharacterActions characterActions)
    {
        @jump.value = characterActions.jump.value;
        @run.value = characterActions.run.value;
        @interact.value = characterActions.interact.value;
        @jetPack.value = characterActions.jetPack.value;
        @dash.value = characterActions.dash.value;
        @crouch.value = characterActions.crouch.value;
        @attack.value = characterActions.attack.value;
        @grap.value = characterActions.grap.value;
        @test.value = characterActions.test.value;
        @spAttack.value = characterActions.spAttack.value;
        @defend.value = characterActions.defend.value;


        @pitch.value = characterActions.pitch.value;
        @roll.value = characterActions.roll.value;

        @pitch.value = characterActions.pitch.value;
        @roll.value = characterActions.roll.value;
        @movement.value = characterActions.movement.value;
    }

    /// <summary>
    /// Update all the actions internal states.
    /// </summary>
    public void Update(float dt)
    {
        @jump.Update(dt);
        @run.Update(dt);
        @interact.Update(dt);
        @jetPack.Update(dt);
        @dash.Update(dt);
        @crouch.Update(dt);
        @attack.Update(dt);
        @grap.Update(dt);
        @test.Update(dt);
        @spAttack.Update(dt);
        defend.Update(dt);
    }
}
