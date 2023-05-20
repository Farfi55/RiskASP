using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("exchange_cards")]
    public class ExchangeCardsPredicate
    {
        // exchange_cards(T, Player, ExchangeId, ExchangeTypeId, Troops).

        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;
        
        [Param(2)] public int ExchangeId;
        
        [Param(3)] public int ExchangeTypeId;
        
        [Param(4)] public int Troops;
        
        public ExchangeCardsPredicate()
        {
        }
        
        public ExchangeCardsPredicate(int turn, string player, int exchangeId, int exchangeTypeId, int troops)
        {
            Turn = turn;
            Player = player;
            ExchangeId = exchangeId;
            ExchangeTypeId = exchangeTypeId;
            Troops = troops;
        }
        
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setExchangeId(int exchangeId) => ExchangeId = exchangeId;
        public int setExchangeTypeId(int exchangeTypeId) => ExchangeTypeId = exchangeTypeId;
        public int setTroops(int troops) => Troops = troops;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getExchangeId() => ExchangeId;
        public int getExchangeTypeId() => ExchangeTypeId;
        public int getTroops() => Troops;
    }
}