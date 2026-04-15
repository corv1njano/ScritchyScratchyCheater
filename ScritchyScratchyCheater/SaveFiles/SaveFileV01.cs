using ScritchyScratchyCheater.Interfaces;
using System.Text.Json;

namespace ScritchyScratchyCheater.SaveFiles
{
    /// <summary>
    /// JSON structure for save file version 0.1, last used in Scritchy Scratchy version 1.0.30d
    /// </summary>
    internal class SaveFileV01 : ISaveFile
    {
        public string? SaveVersion { get; set; }
        public string? GameVersion { get; set; }
        public long Timestamp { get; set; }
        public long PlayedTime { get; set; }

        public LayerOneDataV01? LayerOne { get; set; }

        public int PrestigeCount { get; set; }
        public int PrestigeCurrency { get; set; }
        public int CurrentAct { get; set; }
        public int DeathByFinalChanceCount { get; set; }
        public bool FadeInFromWhite { get; set; }

        public List<object>? DiamondsGottenFromTicket { get; set; }
        public List<object>? DialoguesPlayed { get; set; }

        public Dictionary<string, int>? BoughtPrestigeUpgrades { get; set; }

        public int TotalPrestigeCurrencySpent { get; set; }

        public object? ActiveChallenge { get; set; }

        public List<object>? CompletedChallenges { get; set; }
        public List<object>? AchievementsGotten { get; set; }
        public List<object>? AchievementsClaimed { get; set; }

        public List<object>? UnlockedCosmetics { get; set; }
        public List<object>? BoughtCosmetics { get; set; }
        public List<object>? EquippedCosmetics { get; set; }

        public List<object>? CompletedOnboardingSteps { get; set; }
        public List<object>? DlcUnlocked { get; set; }

        public bool IsPrestiging { get; set; }
        public bool DeathByFinalChance { get; set; }

        public int DeathCount { get; set; }
        public int LoanCount { get; set; }

        public double Tokens { get; set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this, App.JsonOptions);
        }
    }

    internal sealed class LayerOneDataV01
    {
        public double Money { get; set; }
        public double TimeSpentInThisPrestige { get; set; }

        public Dictionary<string, TicketDataV01>? TicketProgressionDict { get; set; }
        public Dictionary<string, UpgradeDataV01>? UpgradeDataDict { get; set; }

        public List<object>? TableItems { get; set; }
        public List<object>? TableItemSaves { get; set; }

        public List<object>? JackpotsGotten { get; set; }
        public List<object>? SuperJackpotsGotten { get; set; }

        public double LastUnlockedProgressionGoal { get; set; }
        public double TotalMoneyEarnedThisProgressionGoal { get; set; }

        public List<object>? ClaimedCustomTableItems { get; set; }

        public bool FirstTicketOpened { get; set; }

        public List<object>? Loans { get; set; }

        public bool BankruptcyWarningGiven { get; set; }
        public bool InitializedChallenge { get; set; }
        public bool InitializedPerks { get; set; }

        public int MachineTier { get; set; }
        public int MachineFeedCount { get; set; }

        public double MachineProcessingTimeLeft { get; set; }

        public int Souls { get; set; }

        public object? LastTicketUnlocked { get; set; }

        public double ElectricFanChargeLeft { get; set; }
        public bool FanPaused { get; set; }

        public double EggTimerChargeLeft { get; set; }

        public bool MundoDead { get; set; }
        public bool TrashCanDead { get; set; }

        public bool BoughtScratchOff { get; set; }
    }

    internal sealed class TicketDataV01
    {
        public string? Id { get; set; }
        public int Xp { get; set; }
        public int Level { get; set; }
    }

    internal sealed class UpgradeDataV01
    {
        public string? Id { get; set; }
        public int BuyCount { get; set; }
    }
}
