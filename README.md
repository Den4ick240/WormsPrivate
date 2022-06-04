# WormsPrivate

Worms project is a simulator of worm world where worms can move, eat and reproduce. The goal is to create a worm behaviour that will result in having the most warms by 100th step.

Worms.Web project contains a web server that provides the action that a worm should take in order to have the most worms by the 100th step. The simulator asks this server for action for every worm on every step.

This world lasts for 100 steps. On the first step there is one worm. A worm has a location in integer x y coordinates of the world. Initialy every worm has 10 live points. Every worm loses 1 live point every step. Every step worm can choose to go one step in any direction, reproduce(loosing 10 live points) or do nothing. Every step a new food item appeares in a random point of the world. When worm location matches the food location, the worm eats the food and gains 10 live points. Worm can't move to a point that is already occupied by another worm.
