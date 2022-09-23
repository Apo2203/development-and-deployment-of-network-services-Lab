# development and deployment of network services-Lab
 An implementation of client-servers interaction

Particularly I have to implement this kind of "real" scenario:

Components: Wolf (server), rabbit (client1), water(client2)

Description: When rabbit is born it generates a random weight for itself. Afterwards rabbit generates random values that specify distance of the rabbit to the wolf. If the distance is less than chosen, wolf eats rabbit and gets the amount of food equal to the rabbits weight. Eaten rabbit dies and gets reborn after 5 sec. When water appears it generates random coordinates in a two dimensional space and volume for itself. Wolf moves around by generating random coordinates in a two dimensional space for itself. If wolf comes to water closer than chosen distance, it consumes that water and gets amount of food equal to the volume of water drunken. Consumed water disappears and reappears after 5 sec. When wolf consumes more food than chosen amount, it can neither eat nor drink for 5 sec. and resets to zero amount of food afterwards.