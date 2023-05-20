using it.unical.mat.embasp.languages;

namespace EmbASP.predicates
{
    [Id("cards_count")]
    public class CardsCountPredicate
    {
        // cards_count(T, Player, Cards).

        [Param(0)] public int Turn;
        
        [Param(1)] public string Player;
        
        [Param(2)] public int Cards;

        public CardsCountPredicate()
        {
        }
        
        public CardsCountPredicate(int turn, string player, int cards)
        {
            Turn = turn;
            Player = player;
            Cards = cards;
        }
        
        public int setTurn(int turn) => Turn = turn;
        public string setPlayer(string player) => Player = player;
        public int setCards(int cards) => Cards = cards;
        
        public int getTurn() => Turn;
        public string getPlayer() => Player;
        public int getCards() => Cards;
    }
}