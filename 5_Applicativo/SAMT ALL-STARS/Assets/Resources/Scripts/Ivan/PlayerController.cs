using System;
using Resources.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

// Il "cervello" del player. Gestisce input, movimento, salto, dash, attacchi e vita.
// Esiste un'istanza per ogni giocatore in scena
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Transform))]
public class PlayerController : MonoBehaviour
{
    [Header("Test / Default")]
    [SerializeField] private string defaultCharacter = "ivan";

    // UI: nome e HP del player nella barra in alto.
    private TextMeshProUGUI hpText;
    private TextMeshProUGUI nameText;

    private Keyboard keyboard;
    private Gamepad gamepad;

    private CharacterManager characterManager;
    private Character character;
    private InputType inputType;
    private string playerName;
<<<<<<< Updated upstream
    private string opponentName;

=======
    
>>>>>>> Stashed changes
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int OnGround = Animator.StringToHash("OnGround");
    private static readonly int IsDashing = Animator.StringToHash("IsDashing");

    public float speed = 1.5f;
    public float jumpPower = 10f;

    public LayerMask groundLayer;
    private bool onGround;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private new Transform transform;

    private float dashPower = 24f;
    private float dashTime = 0.1f;
    private float dashCooldown = 0.5f;
    private bool canDash = true;
    private bool isDashing;

    // Stato dell'animazione di attacco in corso. Quando isStartedAnimation = true il personaggio sta tirando un colpo e non puo' fare altro fino al timeout.
    private bool isStartedAnimation;
    private float time;
    public float animationTime;
    private string typeAnimation;

    private Animator animator;

    [Header("Hitbox")]
    public Hitbox punchHitbox;
    public Hitbox kickHitbox;
    
    private float lastDamageVoiceTime = -999f;
    private const float DAMAGE_VOICE_COOLDOWN = 2.5f;

    public void Awake()
    {
        // Vuoto per ora. Tutto il setup sta in Start.
    }

    // Iscrizione/disiscrizione all'evento dei colpi.
    private void OnEnable()
    {
        CombatEvents.OnHit += HandleHit;
    }

    private void OnDisable()
    {
        CombatEvents.OnHit -= HandleHit;
    }

    // Il listener degli attacchi. Viene chiamato per OGNI colpo del gioco, quindi si controlla anche il target
    private void HandleHit(DamageInfo info)
    {
        if (info.Target != gameObject) return;
        TakeDamage(info.Damage);
    }

    void Start()
    {
        // Pad
        gamepad = new Gamepad();
        gamepad.Gameplay.Dash.performed  += ctx => Dash(InputType.Controller);
        gamepad.Gameplay.Jump.performed  += ctx => Jump(InputType.Controller);
        gamepad.Gameplay.Kick.performed  += ctx => Kick(InputType.Controller);
        gamepad.Gameplay.Punch.performed += ctx => Punch(InputType.Controller);
        gamepad.Gameplay.Move.performed  += ctx => Movement(InputType.Controller, ctx.ReadValue<float>());
        gamepad.Gameplay.Move.canceled   += ctx => Movement(InputType.Controller, 0);

        // Tastiera
        keyboard = new Keyboard();
        keyboard.Gameplay.Dash.performed  += ctx => Dash(InputType.Keyboard);
        keyboard.Gameplay.Jump.performed  += ctx => Jump(InputType.Keyboard);
        keyboard.Gameplay.Kick.performed  += ctx => Kick(InputType.Keyboard);
        keyboard.Gameplay.Punch.performed += ctx => Punch(InputType.Keyboard);
        keyboard.Gameplay.Move.performed  += ctx => Movement(InputType.Keyboard, ctx.ReadValue<float>());
        keyboard.Gameplay.Move.canceled   += ctx => Movement(InputType.Keyboard, 0);

        OnEnableKeyboard();
        OnEnableGamepad();

        characterManager = GameManager.Instance.characterManager;
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        onGround = false;
        animator = GetComponent<Animator>();
        isStartedAnimation = false;
        
        if (gameObject.name == "Player_Joystick")
        {
            inputType = InputType.Controller;
            playerName = "Player2";
            opponentName = "Player1";
            nameText = GameObject.Find("Player_2").GetComponent<TextMeshProUGUI>();
            hpText  = GameObject.Find("Hp_Player_2").GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.Log("ci entra");
            inputType = InputType.Keyboard;
            playerName = "Player1";
            opponentName = "Player2";
            nameText = GameObject.Find("Player_1").GetComponent<TextMeshProUGUI>();
            hpText  = GameObject.Find("Hp_Player_1").GetComponent<TextMeshProUGUI>();
        }

        // Carica il personaggio scelto in CharacterSelection.
        string chosen = PlayerPrefs.GetString(playerName, defaultCharacter);
        character = characterManager.getCharByName(chosen);

        if (character == null)
        {
            LogManager.Error($"Personaggio '{chosen}' non trovato per {playerName}. Controlla il nome.");
            return;
        }

        animator.runtimeAnimatorController = character.Controller;
        spriteRenderer.sprite = character.Sprite;
        nameText.text = character.Data.Name;
<<<<<<< Updated upstream
        character.CurrentHp = character.Data.maxHp;
        hpText.text = character.CurrentHp + "Hp";
=======
        hpText.text = character.Data.MaxHp + "Hp";

        LogManager.Info($"{playerName} pronto come {character.Data.Name} ({character.CurrentHp} HP)");
>>>>>>> Stashed changes
    }

    void Update()
    {
        // Timer dell'animazione di attacco in corso: quando scade, libera il personaggio e spegne il bool dell'Animator cosi torna in stato neutro.
        if (isStartedAnimation)
        {
            time += Time.deltaTime;
            if (time > animationTime)
            {
                isStartedAnimation = false;
                animator.SetBool(typeAnimation, false);
                typeAnimation = "";
            }
        }
        
        if (Mathf.Abs(rb.linearVelocityX) > 0.01 && !isStartedAnimation && onGround && !isDashing)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (!string.IsNullOrEmpty(typeAnimation))
            animator.SetBool(typeAnimation, isStartedAnimation);
    }

    private void Dash(InputType inputTry)
    {
        if (inputTry == inputType && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void Punch(InputType inputTry)
    {
        if (inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Punch";
        time = 0;
    }

    private void Kick(InputType inputTry)
    {
        if (inputTry != inputType || isStartedAnimation) return;
        isStartedAnimation = true;
        typeAnimation = "Kick";
        time = 0;
    }

    private void Jump(InputType inputTry)
    {
        if (inputTry != inputType || !onGround) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        onGround = false;
        animator.SetBool(OnGround, onGround);
        animator.SetBool(IsJumping, true);
        AudioManager.Instance.PlaySFX("jump");
    }

    private void Movement(InputType inputTry, float move)
    {
        if (inputTry != inputType || isDashing) return;
        if (move > 0)      SetFacing(true);
        else if (move < 0) SetFacing(false);
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    // Gira l'intero player
    private void SetFacing(bool faceRight)
    {
        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * (faceRight ? 1f : -1f);
        transform.localScale = s;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (groundLayer == (1 << other.gameObject.layer))
        {
            onGround = true;
            animator.SetBool(IsJumping, false);
            animator.SetBool(OnGround, onGround);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Empty"))
        {
            Die();
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if ((groundLayer == (1 << other.gameObject.layer)))
        {
            onGround = false;
            animator.SetBool(OnGround, onGround);
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        AudioManager.Instance.PlaySFX("dash");
        animator.SetBool(IsDashing, isDashing);

<<<<<<< Updated upstream
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

=======
        // Spegne la gravita' durante il dash cosi il personaggio sfreccia
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        // Direzione del dash: dipende da come sto guardando
>>>>>>> Stashed changes
        float direction = transform.localScale.x < 0 ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * dashPower, 0f);

        yield return new WaitForSeconds(dashTime);

<<<<<<< Updated upstream
        rb.linearVelocity = new Vector2(0f, 0f);

=======
>>>>>>> Stashed changes
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool(IsDashing, isDashing);

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void TakeDamage(int damage)
    {
        character.CurrentHp -= damage;
<<<<<<< Updated upstream
        
        Debug.Log($"{character.Data.Name} ha {character.CurrentHp} HP");
=======
        LogManager.Info($"{character.Data.Name} ({playerName}) ha {character.CurrentHp} HP");
>>>>>>> Stashed changes

        if (character.CurrentHp <= 0) Die();
        hpText.text = character.CurrentHp + "Hp";
        
        if (character.CurrentHp > 0 && Time.time - lastDamageVoiceTime >= DAMAGE_VOICE_COOLDOWN)
        {
            lastDamageVoiceTime = Time.time;
            AudioManager.Instance.PlayVoice(character.Data.Name, VoiceLine.Damage);
        }
    }

    private void Die()
    {
<<<<<<< Updated upstream
        PlayerPrefs.SetString("Loser", character.Data.Name);
        PlayerPrefs.SetString("Winner", PlayerPrefs.GetString(opponentName, defaultCharacter));
        gamepad.Disable();
        keyboard.Disable();
        UIManager.openPostMatch();
=======
        LogManager.Info($"{character.Data.Name} ({playerName}) e' KO");
        AudioManager.Instance.PlaySFX("win");

        // La voiceline di vittoria la dice L'AVVERSARIO, non chi muore.
        PlayerController[] all = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (PlayerController p in all)
        {
            if (p == this) continue;
            if (p.character == null) continue;
            AudioManager.Instance.PlayVoice(p.character.Data.Name, VoiceLine.Victory);
            break;
        }
>>>>>>> Stashed changes
    }

    // Abilitazione/disabilitazione input e hitbox.
    private void OnEnableKeyboard()  => keyboard.Gameplay.Enable();
    private void OnDisableKeyboard() => keyboard.Gameplay.Disable();
    private void OnEnableGamepad()   => gamepad.Gameplay.Enable();
    private void OnDisableGamepad()  => gamepad.Gameplay.Disable();
    public void EnablePunchHitbox()  => punchHitbox.EnableHitbox();
    public void DisablePunchHitbox() => punchHitbox.DisableHitbox();
    public void EnableKickHitbox()   => kickHitbox.EnableHitbox();
    public void DisableKickHitbox()  => kickHitbox.DisableHitbox();
}
