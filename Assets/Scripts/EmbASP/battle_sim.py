import random

def d6():
    return random.randint(1, 6)

def battle(n_attacker: int, n_defender: int) -> tuple():
    attacker_rolls = [d6() for _ in range(n_attacker)]
    defender_rolls = [d6() for _ in range(n_defender)]
    attacker_rolls.sort(reverse=True)
    defender_rolls.sort(reverse=True)
    attacker_losses = 0
    defender_losses = 0
    for i in range(min(n_attacker, n_defender)):
        if attacker_rolls[i] > defender_rolls[i]:
            defender_losses += 1
        else:
            attacker_losses += 1
    return (attacker_losses, defender_losses)

def full_battle(n_attacker: int, n_defender: int) -> tuple():
    while n_attacker > 0 and n_defender > 0:
        attacking_troops = min(n_attacker, 3)
        defending_troops = min(n_defender, 3)

        (attacker_loss, defender_loss) = battle(attacking_troops, defending_troops)
        n_attacker -= attacker_loss
        n_defender -= defender_loss
    
    return (n_attacker, n_defender)

def battle_sim(n_attacker: int, n_defender: int, n_simulations: int) -> float:
    attacker_wins = 0
    for _ in range(n_simulations):
        (attacker, defender) = full_battle(n_attacker, n_defender)
        if attacker > defender:
            attacker_wins += 1
    return attacker_wins / n_simulations

def simulate_all_battles(n_simulations: int, max_attacker: 20, max_defender: 20, 
                         min_attacker: int = 0, min_defender: int = 1):
    results = []
    for i in range(min_attacker, max_attacker + 1):
        for j in range(min_defender, max_defender + 1):
            result = (i, j, battle_sim(i, j, n_simulations))
            print_result(result)
            results.append(result)

    return results


def print_result(result):
    (attacker, defender, win_rate) = result
    win_rate_int = round(win_rate * 1000)
    win_rate_def = 1000 - win_rate_int
    
    if defender == 1:
        print()
    print(f"battle_chance({attacker},{defender: >3},{win_rate_int: >5},{win_rate_def: >5}).")

    



def main():
    n_simulations = 5000
    simulate_all_battles(n_simulations, 90, 70, 50, 0)


if __name__ == "__main__":
    main()




