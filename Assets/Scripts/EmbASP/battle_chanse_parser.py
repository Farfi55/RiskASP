import re



def print_result(result):
    (attacker, defender, win_rate) = result
    win_rate_def = 1000 - win_rate
    print(f"battle_chance({attacker},{defender: >3},{win_rate: >5},{win_rate_def: >5}).")


p = re.compile("battle_chance\((\d+), *?(\d+), *?(\d+)\)")





while True:
    line = input()
    # regex that matches battle_chance({attacker},{defender},{win_rate_int})
    match = p.match(line)
    if(not match):
        print(line)
        continue
    (attacker, defender, win_rate) = match.groups()
    attacker = int(attacker)
    defender = int(defender)
    win_rate = int(win_rate)
    print_result((attacker, defender, win_rate))
    

