using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    private Rigidbody _rb;
    private SphereCollider _sphereCollider;

    private bool _isCast;
    private float _castTime;
    private bool _kickedByPlayer;
    
    private event Action<int, Dice> onActivate;
    private CancellationTokenSource _cancellationTokenSource;

    private bool _wasInit;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_wasInit)
            return;
        
        _rb = GetComponent<Rigidbody>();
        _sphereCollider = GetComponent<SphereCollider>();

        _cancellationTokenSource = new CancellationTokenSource();

        _wasInit = true;
    }

    private void FixedUpdate()
    {
        _sphereCollider.radius = Mathf.Lerp(0.5f, 1.5f, _rb.velocity.magnitude * 0.3f);
        _rb.AddForce(Physics.gravity * 0.1f);
        
        if (_isCast && Time.time - _castTime > 1f && _rb.velocity.magnitude < 0.1f)
        {
            var face = GetFace();
            if (face == 0)
            {
                _rb.AddForce(new Vector3(Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f), Random.Range(0.5f, 1.0f)),
                    ForceMode.Impulse);
                _castTime = Time.time;
                return;
            }

            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _isCast = false;
            _kickedByPlayer = false;

            _cancellationTokenSource = new CancellationTokenSource();
            OnLand();
        }
    }

    private async void OnLand()
    {
        var face = GetFace();
        var param = DiceManager.Instance.diceLandParameters;
        
        // TODO play land sfx
        
        // display number
        var num = Instantiate(DiceManager.Instance.displayPrefab, FindObjectOfType<Canvas>().transform);
        num.transform.position = transform.position;
        
        // Apply color
        var top = Utils.LerpHSV(param.startColor, param.endColor, (face - 1) / 5f, false);
        var bottom = Utils.LerpHSV(param.startColor, param.endColor, (face - 1.5f) / 5f, false);

        var text = num.GetComponent<TMP_Text>();
        text.colorGradient = new VertexGradient(top, top, bottom, bottom);
        text.text = face.ToString();
        
        // Animate
        text.color = Color.clear;
        ;
        await UniTask.WhenAll(num.transform.DOMoveY(transform.position.y + param.displayOffset, param.fadeInTime).ToUniTask(),
            text.DOColor(Color.white, param.fadeInTime).ToUniTask());

        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(param.timeToActivation), false, PlayerLoopTiming.Update, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            if (_cancellationTokenSource.IsCancellationRequested)
            {
                text.DOColor(Color.clear, param.fadeOutTime);
                await num.transform.DOMoveY(transform.position.y + 2 * param.displayOffset, param.fadeOutTime);
                Destroy(num);
                return;
            }
        }

        // TODO play activation sfx
        text.DOColor(Color.clear, param.fadeOutTime);
        await num.transform.DOMoveY(transform.position.y + 2 * param.displayOffset, param.fadeOutTime);
        Destroy(num);
        
        if (_cancellationTokenSource.IsCancellationRequested == false)
            onActivate?.Invoke(face, this);
    }

    private int GetFace()
    {
        int count = 0;
        float threshold = 0.9f;
        
        if (Vector3.Dot (transform.forward, Vector3.back) > threshold)
            count = 2;
        if (Vector3.Dot (-transform.forward, Vector3.back) > threshold)
            count = 5;
        if (Vector3.Dot (transform.up, Vector3.back) > threshold)
            count = 1;
        if (Vector3.Dot (-transform.up, Vector3.back) > threshold)
            count = 6;
        if (Vector3.Dot (transform.right, Vector3.back) > threshold)
            count = 3;
        if (Vector3.Dot (-transform.right, Vector3.back) > threshold)
            count = 4;

        return count;
    }

    public void CastDice(Vector3 v, Action<int, Dice> callback, bool kickedByPlayer = false)
    {
        Init();
        
        _rb.constraints = RigidbodyConstraints.None;
        _rb.AddForce(v, ForceMode.Impulse);
        _castTime = Time.time;
        _isCast = true;
        _kickedByPlayer = _kickedByPlayer || kickedByPlayer;

        _cancellationTokenSource.Cancel();

        if (callback != null)
        {
            onActivate = null;
            onActivate += callback;
        }
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Cancel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var turret = collision.gameObject.GetComponent<Turret>();
        if (turret != null && _isCast && _kickedByPlayer)
        {
            Destroy(turret.gameObject, 0.05f);
            AudioManager.Instance.PlayEffectRandom("exp");
        }
        
        var dice = collision.gameObject.GetComponent<Dice>();
        if (dice != null && _isCast && _kickedByPlayer)
        {
            if (dice._rb.velocity.sqrMagnitude > _rb.velocity.sqrMagnitude)
                return;
            
            Destroy(dice.gameObject, 0.05f);
            AudioManager.Instance.PlayEffectRandom("exp");
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            AudioManager.Instance.PlayEffectRandom("dice_hit");
    }
}
