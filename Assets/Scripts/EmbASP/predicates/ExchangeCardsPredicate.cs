using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("exchange_cards")]
    public class ExchangeCardsPredicate
    {
        // exchange_cards(T, Player, ExchangeId).

        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;
        
        [Param(2)] public int ExchangeId;
        
        public ExchangeCardsPredicate()
        {
        }
        
        public ExchangeCardsPredicate(int turn, string player, int exchangeId)
        {
            Turn = turn;
            Player = player;
            ExchangeId = exchangeId;
        }
        
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setExchangeId(int exchangeId) => ExchangeId = exchangeId;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getExchangeId() => ExchangeId;
        
    }
}