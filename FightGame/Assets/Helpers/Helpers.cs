namespace Assets.Helpers
{
    public class Characters
    {
        public const string Witch = "character_witch";
        public const string Warrior = "character_warrior";
        public const string Enemy = "character_enemy";
    }

    public class CharacterAttacks
    {
        public const string Magic = "characterAttack_magic";
        public const string Shuriken = "characterAttack_shuriken";
    }

    public class CharacterAnimatorParameters
    {
        public const string Movement = "isMoving";
        public const string Death = "isDead";
    }

    public class Scenes
    {
        public const string FightScene = "FightScene";
        public const string TreasureScene = "TreasureScene";
    }

    public class SceneObjects
    {
        public const string SceneManager = "sceneManager";
        public const string TreasureSceneTreasure = "prop_treasure";
        public const string FightSceneScore = "text_score";
    }

    public class FightSceneMessages
    {
        private const string gameOverMessage = "Congrats, {0}! Click on 'Next level' to go to the next level.";
        private const string scoreTextMessage = "Round: {0}\nScore: {1} - {2}";

        public const string NextRoundMessage = "Click on 'Next round' to go to the next round.";

        public static string GameOverMessage(string winnerName)
        {
            return string.Format(gameOverMessage, winnerName);
        }

        public static string ScoreTextMessage(int round, int firstScore, int secondScore)
        {
            return string.Format(scoreTextMessage, round.ToString(), firstScore.ToString(), secondScore.ToString());
        }
    }

    public class TreasureSceneMessages
    {
        public const string WinMessage = "Congrats! You got the treasure!";
        public const string LoseMessage = "Oh, no! The dragon captured you!";
        public const string GameOverMessage = "Click on 'Play again' to play again or on 'Exit' to exit the game.";
    }

    public class Tags
    {
        public const string Character = "character";
        public const string Obstacle = "obstacle";
        public const string Light = "light";
    }

    public class PlayerPrefsKeys
    {
        public const string FightSceneWinner = "fight_scene_winner";
        public const string FightSceneRound = "fight_scene_round";
        public const string FightSceneWitchScore = "fight_scene_witch_score";
        public const string FightSceneWarriorScore = "fight_scene_warrior_score";
    }

    public class PopupButtons
    {
        public const string OK = "OK";
        public const string NextLevel = "Next level";
        public const string Exit = "Exit";
        public const string PlayAgain = "Play again";
        public const string NextRound = "Next round";
    }

    public class ResourceFiles
    {
        public const string FightSceneInstructions = "FightSceneInstructions";
        public const string TreasureSceneInstructions = "TreasureSceneInstructions";
    }

    public class HealthBar
    {
        private const string healthBarName = "healthBar_{0}";

        public const int MaxHP = 100;
        public const int MaxStamina = 100;
        public const string HPHealthBar = "hp";
        public const string StaminaHealthBar = "stamina";

        public static string HealthBarName(string characterName)
        {
            return string.Format(healthBarName, characterName);
        }
    }

    public enum CharacterAction : int
    {
        Dodge = 0,
        Attack = 1
    };
}
