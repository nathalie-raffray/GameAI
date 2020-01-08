# Game--AI
Made an AI that solves game "Century: The spice road"

The AI-controlled player is a spice trader, and I created a GOAP planner that allows the character to trade spices. The main goal is to fill a caravan with 2 units of each of 7 spices: Turmeric, Saffron, Cardamom, Cinnamon, Cloves, Pepper and Sumac. In order to do this the player-agent needs to visit traders who each trade one specific spice for another, in various quantities, and deal with a roving thief. The thief may steal up to 2 spices from the trader's inventory or the caravan, and when not stealing, wanders randomly.

The player-agent has an inventory that allows carrying at most 4 units of spice at any one time. The caravan has unlimited capacity, and traders have unlimited supply. In addition to trading and pathfinding actions, and while at the caravan, the player-agent can either transfer any number of items from their inventory to the caravan, or vice versa.

The player-agent is represented by a red cone, and the thief by a green cone. The player-agent's planned sequence of actions is dynamic (can change if the thief steals from him/her) and is depicted to the left of the screen, you can scroll through it. On the right of the screen there is a grid showing the dynamically updated contents of the player-agent's inventory and caravan.

The ultimate goal of the player-agent is to store 2 of each spice in the caravan. Spices are acquired by trading different spices with the traders. The player-agent navigatives to a trader to trade with them. Each trader accepts a type of spice and returns another type. The eight traders are represented by T1, T2, T3, T4,..T8 and trade accordingly:

(a) Trader 1: Gives you 2 turmeric units.

(b) Trader 2: Takes 2 Turmeric units and gives you 1 Saffron unit.

(c) Trader 3: Takes 2 Saffron units and gives you 1 Cardamom unit.

(d) Trader 4: Takes 4 Turmeric units and gives you 1 Cinnamon.

(e) Trader 5: Takes 1 Cardamom and 1 Turmeric and gives you 1 Cloves unit.

(f) Trader 6: Takes 2 Turmeric, 1 Saffron and 1 Cinnamon and gives 1 Pepper unit.

(g) Trader 7: Takes 4 Cardamom units and gives you 1 Sumac unit.

(h) Trader 8: Takes 1 Saffron, 1 Cinnamon and 1 Cloves unit and gives you 1 Sumac unit.

IMPORTANT: To increase simulation speed by a factor of 2, press '+'. Likewise, to decrease simulation speed by a factor of 2, press '-'. For your own sanity you will want to increase the simulation speed to see the game to its end.
