using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    
    [Id("possible_card_exchange")]
    public class PossibleCardExchangePredicate
    {
        // possible_card_exchange(T, Player, ExchangeId, ExchangeTypeId, Card1, Card2, Card3, Troops).
        
        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;
        
        [Param(2)] public int ExchangeId;
        
        [Param(3)] public int ExchangeTypeId;
        
        [Param(4)] public string Card1;
        
        [Param(5)] public string Card2;
        
        [Param(6)] public string Card3;
        
        [Param(7)] public int Troops;
        
        public PossibleCardExchangePredicate()
        {
        }
        
        public PossibleCardExchangePredicate(int turn, string player, int exchangeId, int exchangeTypeId, string card1, string card2, string card3, int troops)
        {
            Turn = turn;
            Player = player;
            ExchangeId = exchangeId;
            ExchangeTypeId = exchangeTypeId;
            Card1 = card1;
            Card2 = card2;
            Card3 = card3;
            Troops = troops;
        }
        
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setExchangeId(int exchangeId) => ExchangeId = exchangeId;
        public int setExchangeTypeId(int exchangeTypeId) => ExchangeTypeId = exchangeTypeId;
        public string setCard1(string card1) => Card1 = card1;
        public string setCard2(string card2) => Card2 = card2;
        public string setCard3(string card3) => Card3 = card3;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getExchangeId() => ExchangeId;
        public int getExchangeTypeId() => ExchangeTypeId;
        public string getCard1() => Card1;
        public string getCard2() => Card2;
        public string getCard3() => Card3;
        public int getTroops() => Troops;
    }
}