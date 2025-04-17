using Fusion;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerProperties : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnHealthChange))]
    public int Health { get; private set; }
    public Slider _slider;

    [Networked]
    public int Score { get; set; } // Thêm thuộc tính Score

    public TextMeshProUGUI scoreText; // Thêm reference đến UI Text để hiển thị điểm

    private CameraSwitcher _cameraSwitcher; // Tham chiếu đến CameraSwitcher
    public bool isDead = false; // Biến để theo dõi trạng thái sống/chết

    private void OnHealthChange()
    {
        _slider.value = Health;
        Debug.Log($"Health change to {Health}");
        if (Health <= 0 && !isDead)
        {
            isDead = true; // Đánh dấu là đã chết
            Debug.Log("Player is dead");
            // Gọi phương thức trong CameraSwitcher để chuyển camera
            if (_cameraSwitcher != null)
            {
                _cameraSwitcher.OnPlayerDeath(this);
            }
        }
    }

    private void Start()
    {
        Health = 10;
        // Tìm CameraSwitcher trong scene
        _cameraSwitcher = FindObjectOfType<CameraSwitcher>();

        // Khởi tạo điểm số từ PlayerPrefs
        if (HasStateAuthority)
        {
            Score = PlayerPrefs.GetInt("PlayerScore", 0);
            UpdateScoreDisplay();
        }
    }

    void Update()
    {
        if (HasStateAuthority && !isDead) // Kiểm tra trạng thái sống
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Health -= 1;
            }
        }
    }

    public void TakeDMG(int damage)
    {
        if (HasStateAuthority)
        {
            Health = Mathf.Max(0, Health, -damage);
        }
    }

    // Phương thức tăng điểm
    public void AddScore(int amount)
    {
        if (HasStateAuthority)
        {
            Score += amount;
            PlayerPrefs.SetInt("PlayerScore", Score);
            PlayerPrefs.Save();
            UpdateScoreDisplay();
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "" + Score.ToString();
        }
    }

    public void ResetScore()
    {
        if (HasStateAuthority)
        {
            Score = 0;
            PlayerPrefs.SetInt("PlayerScore", Score);
            PlayerPrefs.Save();
            UpdateScoreDisplay();
        }
    }
}