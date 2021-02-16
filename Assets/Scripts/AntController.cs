﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour{
    private bool _facingRight = false;

    [SerializeField]
    private float _speed = 5f;

    private Rigidbody2D _rigidbody;

    private Animator _animator;

    private float _horizontalMove = 0f;

    private float _time = 0f;

    // Start is called before the first frame update
    void Start() {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        _time += Time.deltaTime;

        if(Mathf.CeilToInt(_time) % 2 == 1) {
            _horizontalMove = 1f; // Input.GetAxis("Horizontal");
        }
        else {
            _horizontalMove = -1f; // Input.GetAxis("Horizontal");
        }
        
    }

    void FixedUpdate() {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            
            LevelManager.LevelManagerInstance.CollectAnt(this.gameObject.GetInstanceID());

            Destroy();
        }
    }

    private void Move() {
        // Handles the walking/running animation. Should likely be moved to a function but will
        // worry about it when/if there are more animations.
        if (_horizontalMove != 0) {
            _animator.SetBool("walking", true);
        }
        else {
            _animator.SetBool("walking", false);
        }

        // Set the velocity to the frame normalized time * the input on the x and maintain the y
        // velocity so that gravity works correctly.
        Vector2 velocity = new Vector2(
            _horizontalMove * _speed * Time.fixedDeltaTime * 10,
            _rigidbody.velocity.y
        );

        // Update the velocity with the player input velocity
        _rigidbody.velocity = velocity;

        CheckIfShouldFlip(velocity.x);
    }

    private void CheckIfShouldFlip(float xInput) {
        if (xInput > 0 && !_facingRight) {
            Flip();
        }
        else if (xInput < 0 && _facingRight) {
            Flip();
        }
    }

    private void Flip() {
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void Destroy() {
        this.gameObject.SetActive(false);

        Destroy(gameObject, 5);
    }
}