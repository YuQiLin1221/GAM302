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
    public ObjectPool itemPool;
    public TextMeshProUGUI TimeTextUI;

    [Networked] public bool IsHost { get; set; }

    [Networked] private TickTimer gameTimer { get; set; } // Sử dụng TickTimer của Fusion
    private const int GAME_DURATION = 60; // Thời lượng game là 10 giây

    private void Awake()
    {
        // Thêm các dòng này để tự động tìm các đối tượng nếu chưa được gán
        if (countdownText == null) countdownText = GameObject.Find("CountdownText")?.GetComponent<TextMeshProUGUI>();
        if (startButton == null) startButton = GameObject.Find("StartButton")?.GetComponent<Button>();
        if (TimeTextUI == null) TimeTextUI = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        if (itemPool == null) itemPool = FindObjectOfType<ObjectPool>();

        PlayerPrefs.DeleteKey("PlayerScore");
    }
    private void Start()
    {
        Time.timeScale = 0;

        // Kiểm tra kỹ hơn trước khi thêm listener
        if (startButton != null)
        {
            startButton.onClick.RemoveAllListeners(); // Xóa các listener cũ
            startButton.onClick.AddListener(OnStartButtonClicked);
            startButton.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("StartButton reference is missing in GameManager!");
        }

        ResetAllScores();
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
                ResetAllScores();
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

        // Bắt đầu đếm thời gian game
        if (Object.HasStateAuthority)
        {
            gameTimer = TickTimer.CreateFromSeconds(Runner, GAME_DURATION);
        }

        yield return new WaitForSeconds(3);
        SpawnItem();
    }

    public void PlayerJoined(PlayerProperties player)
    {
        if (player != null && !players.Contains(player))
        {
            players.Add(player);
            Debug.Log($"{player.name} đã tham gia.");

            // Nếu là người chơi đầu tiên (host) và có state authority
            if (players.Count == 1 && Object.HasStateAuthority)
            {
                IsHost = true;
                startButton.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Attempted to add null or duplicate player");
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        // Cập nhật đồng hồ đếm ngược
        if (gameStarted && gameTimer.IsRunning)
        {
            float remainingTime = gameTimer.RemainingTime(Runner) ?? 0;
            UpdateTimeDisplay(remainingTime);

            if (remainingTime <= 0)
            {
                gameTimer = TickTimer.None;
                EndGame();
            }
        }
    }

    private void UpdateTimeDisplay(float timeInSeconds)
    {
        if (TimeTextUI != null)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60);
            int seconds = Mathf.FloorToInt(timeInSeconds % 60);
            TimeTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    private void EndGame()
    {
        Debug.Log("Game Over!");
        gameStarted = false;
        // Thêm logic kết thúc game ở đây
        if (Object.HasStateAuthority)
        {
            // Gọi RPC để đồng bộ trạng thái kết thúc game với tất cả clients
            RPC_EndGame();
        }
    }

    private void SpawnItem()
    {
        if (Object.HasStateAuthority && itemPool != null)
        {
            try
            {
                NetworkObject item = itemPool.GetNetworkObject();
                if (item == null)
                {
                    Debug.LogError("Failed to get item from pool");
                    return;
                }

                // Thêm code spawn item tại đây
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error spawning item: {e.Message}");
            }
        }
    }

    private void ResetAllScores()
    {
        // Kiểm tra null cho Object trước
        if (Object != null && Object.HasStateAuthority)
        {
            // Kiểm tra null và có player nào không
            if (players != null && players.Count > 0)
            {
                foreach (PlayerProperties player in players)
                {
                    if (player != null)
                    {
                        player.ResetScore();
                    }
                }
            }

            PlayerPrefs.DeleteKey("PlayerScore");
            PlayerPrefs.Save();
        }
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_EndGame()
    {
        Debug.Log("Game Over!");
        gameStarted = false;
        Time.timeScale = 0; // Dừng tất cả physics và animations

        // Có thể thêm các hiệu ứng/kết quả game over ở đây
    }
}