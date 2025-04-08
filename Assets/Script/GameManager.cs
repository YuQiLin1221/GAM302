using Fusion;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : NetworkBehaviour // Kế thừa từ NetworkBehaviour
{
    public List<PlayerProperties> players = new List<PlayerProperties>();
    public TextMeshProUGUI countdownText; // Text để hiển thị đếm ngược
    public Button startButton; // Thêm biến cho nút Start
    private bool gameStarted = false;

    public GameObject itemPrefab; // Prefab của item

    private void Start()
    {
        // Đặt time scale bằng 0 khi bắt đầu
        Time.timeScale = 0;

        // Gán sự kiện cho nút Start
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    public void OnStartButtonClicked()
    {
        startButton.gameObject.SetActive(false);
        StartGame(); // Gọi phương thức StartGame khi nút được nhấn
    }

    public void StartGame()
    {
        if (Object.HasStateAuthority) // Sử dụng Object.HasStateAuthority
        {
            if (players.Count >= 1) // Kiểm tra số lượng người chơi
            {
                StartCoroutine(Countdown(3));
                Time.timeScale = 1;// Bắt đầu đếm ngược 3 giây
            }
            else
            {
                Debug.Log("Chưa đủ người chơi để bắt đầu.");
            }
        }
    }

    public IEnumerator Countdown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            countdownText.text = i.ToString(); // Cập nhật text đếm ngược
            yield return new WaitForSeconds(1); // Chờ 1 giây
        }

        countdownText.text = "Go!"; // Hiển thị "Go!" sau khi đếm ngược
        Time.timeScale = 1; // Bắt đầu trò chơi
        gameStarted = true;
        Debug.Log("Trò chơi bắt đầu!");
        countdownText.gameObject.SetActive(false);

        SpawnItem();
    }

    public void PlayerJoined(PlayerProperties player)
    {
        players.Add(player);
        Debug.Log($"{player.name} đã tham gia.");
    }

    private void SpawnItem()
    {
        if (Object.HasStateAuthority)
        {
            for (int i = 0; i < 5; i++) // Lặp 5 lần để spawn 5 items
            {
                // Tạo vị trí ngẫu nhiên trong một phạm vi nhất định
                Vector3 randomPosition = new Vector3(
                    Random.Range(-10f, 10f), // X
                    1f, // Y (đặt ở độ cao 1 để không bị chìm dưới đất)
                    Random.Range(-10f, 10f)  // Z
                );

                // Spawn item
                Runner.Spawn(itemPrefab, randomPosition, Quaternion.identity);
            }
        }
    }
}