using Cards;
using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{

    [Id("card")]
    public class CardPredicate
    {
        // card(T, Player, CardName, CardType, CardTerritory).

        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;
        
        [Param(2)] public string CardName;
        
        [Param(3)] public string CardType;
        
        [Param(4)] public string CardTerritory;
        
        public CardPredicate()
        {
        }
        
        public CardPredicate(int turn, string player, string cardName, string cardType, string cardTerritory)
        {
            Turn = turn;
            Player = player;
            CardName = cardName;
            CardType = cardType;
            CardTerritory = cardTerritory;
        }
        
        public CardPredicate(int turn, string player, Card card)
        {
            Turn = turn;
            Player = player;
            CardName = card.Name;
            CardType = card.Type.ToString();
            CardTerritory = card.Territory != null ? card.Territory.Name : "";
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public string setCardName(string cardName) => CardName = cardName;
        public string setCardType(string cardType) => CardType = cardType;
        public string setCardTerritory(string cardTerritory) => CardTerritory = cardTerritory;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public string getCardName() => CardName;
        public string getCardType() => CardType;
        public string getCardTerritory() => CardTerritory;



    }
}