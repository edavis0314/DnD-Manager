#include <iostream>
#include <cstdlib>
#include <ctime>
#include <conio.h>
#include <windows.h>

int RNG(int sides_max, int sides_min, int number);
int DarkerDungeon();
int CharacterRace(int n);
int CharacterBackground(int n);
int CharacterClass(int n);
int CharacterStats(int str, int dex, int con, int inl, int wis, int chr);
int CharacterAppearance(int n);
int CharacterMotivationAndHabits(int n);

int CharacterID = 0;
int PlayerID = 0;
int SessionNumber = 0;
int RollCount = 0;

using namespace std;

void main() {
	int check = 0;
	int temp = 0;

	cout << "welcome to the Dnd Player and DM Aid!    ";
	srand(time(NULL));
	temp = rand();
	SessionNumber = (temp + 1) % 9999 + 1;

	do {
		check = DarkerDungeon();
		cout << "Are You Done? \n";
		cin >> check;
	} while (check != 1);

	return;
}

int RNG(int sides_max, int sides_min, int number) {
	int MyRoll = 0;
	double valueTotal = 0;
	double outcome = 0;
	srand(time(NULL));

	for (int i = 0; i < number; i++) {
		RollCount++;
		MyRoll = rand();
		if (sides_max == 100 && sides_min == 1) {
			valueTotal = (((CharacterID ^ (PlayerID % SessionNumber) % MyRoll) ^ RollCount)) % 10 + 0;
			outcome = outcome + valueTotal;

			MyRoll = rand();
			valueTotal = (((CharacterID % MyRoll) ^ RollCount)) % 10 + 0;
			outcome = outcome + (valueTotal*10);

			if (outcome == 0) { outcome = 100; }
		}
		else {
			valueTotal = (((CharacterID ^ (PlayerID % SessionNumber) % MyRoll) ^ RollCount)) % sides_max + sides_min;
			outcome = outcome + valueTotal;
		}
	}
	return outcome;
}

int CharacterRace(int n) {
	int temporary = 0;

	temporary = RNG(100, 1, 1);
	if (temporary == 1) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 33) { cout << "Fallen Aasimar*";}
		else if (n >= 34 && n <= 67) { cout << "Protector Aasimar*";}
		else if (n >= 68 && n <= 100) { cout << "Scourge Aasimar*";}
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 2 && temporary <= 4) { cout << "Dragonborn*";}
	else if (temporary >= 5 && temporary <= 19) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 50) { cout << "Hill Dwarf*";}
		else if (n >= 51 && n <= 100) { cout << "Mountain Dwarf*";}
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 20 && temporary <= 29) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 10) { cout << "Drow Elf*";}
		else if (n >= 11 && n <= 55) { cout << "High Elf*";}
		else if (n >= 56 && n <= 100) { cout << "Wood Elf*";}
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 30 && temporary <= 31) { cout << "Firbolg*";}
	else if (temporary >= 32 && temporary <= 37) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 50) { cout << "Forest Gnome*";}
		else if (n >= 51 && n <= 100) { cout << "Rock Gnome*";}
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 38 && temporary <= 39) { cout << "Goliath*";}
	else if (temporary == 40) { cout << "Half-Elf*";}
	else if (temporary == 41) { cout << "Half-Orc*";}
	else if (temporary >= 42 && temporary <= 48) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 50) { cout << "Lightfoot Halfling*";}
		else if (n >= 51 && n <= 100) { cout << "Stout Halfling*";}
		else { cout << "ERROR*"; }}
	else if (temporary >= 49 && temporary <= 90) { cout << "Human*";}
	else if (temporary == 91) { cout << "Kenku*";}
	else if (temporary == 92) { cout << "Lizardfolk*";}
	else if (temporary == 93) {
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 10) { cout << "Bugbear*";}
		else if (n >= 11 && n <= 35) { cout << "Goblin*";}
		else if (n >= 36 && n <= 50) { cout << "Hobgoblim*";}
		else if (n >= 51 && n <= 75) { cout << "Kobold*";}
		else if (n >= 79 && n <= 90) { cout << "Orc*";}
		else if (n >= 91 && n <= 100) { cout << "Yuan-ti Pureblood*";}
		else { cout << "ERROR*"; }
	}
	else if (temporary == 94) { cout << "Tabaxi*";}
	else if (temporary >= 95 && temporary <= 98) { cout << "Tiefling*";}
	else if (temporary == 99) { cout << "Triton*";}
	else if (temporary == 100) { cout << "ANY RACE*";}
	else { cout << "ERROR*"; }

	return 0;
} 

int CharacterBackground(int n) {
	int temporary = 0;
	temporary = RNG(100, 1, 1);

	if (temporary >= 1 && temporary <= 7) { 
		cout << "Acolyte* *";
		temporary = RNG(8, 1, 1);
		if (temporary == 1) { cout << "I idolize a particular hero of my faith, and constantly refer to that person's deeds and example.*"; }
		else if (temporary == 2) { cout << "I can find common ground between the fiercest enemies, empathizing with them and always working toward peace.*"; }
		else if (temporary == 3) { cout << "I see omens in every event and action. The gods try to speak to us, we just need to listen*"; }
		else if (temporary == 4) { cout << "Nothing can shake my optimistic attitude.*"; }
		else if (temporary == 5) { cout << "I quote (or misquote) sacred texts and proverbs in almost every situation.*"; }
		else if (temporary == 6) { cout << "I am tolerant (or intolerant) of other faiths and respect (or condemn) the worship of other gods.*"; }
		else if (temporary == 7) { cout << "I've enjoyed fine food, drink, and high society among my temple's elite.Rough living grates on me.*"; }
		else if (temporary == 8) { cout << "I've spent so long in the temple that I have little practical experience dealing with people in the outside world.*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "Tradition. The ancient traditions o f worship and sacrifice must be preserved and upheld.*Lawful*"; }
		else if (temporary == 2) { cout << "Charity. I always try to help those in need, no matter what the personal cost.*Good*"; }
		else if (temporary == 3) { cout << "Change. We must help bring about the changes the gods are constantly working in the world.*Chaotic*"; }
		else if (temporary == 4) { cout << "Power. I hope to one day rise to the top of my faith's religious hierarchy.*Lawful*"; }
		else if (temporary == 5) { cout << "Faith. I trust that my deity will guide my actions, I have faith that if I work hard, things will go well.*Lawful*"; }
		else if (temporary == 6) { cout << "Aspiration. I seek to prove myself worthy of my god's favor by matching my actions against his or her teachings.*Any*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "I would die to recover an ancient relic of my faith that was lost long ago.*"; }
		else if (temporary == 2) { cout << "I will someday get revenge on the corrupt temple hierarchy who branded me a heretic.*"; }
		else if (temporary == 3) { cout << "I owe my life to the priest who took me in when my parents died.*"; }
		else if (temporary == 4) { cout << "Everything I do is for the common people.*"; }
		else if (temporary == 5) { cout << "I will do anything to protect the temple where I served.*"; }
		else if (temporary == 6) { cout << "I seek to preserve a sacred text that my enemies consider heretical and seek to destroy.*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "I judge others harshly, and myself even more severely.*"; }
		else if (temporary == 2) { cout << "I put too much trust in those who wield power within my temple's hierarchy.*"; }
		else if (temporary == 3) { cout << "My piety sometimes leads me to blindly trust those that profess faith in my god.*"; }
		else if (temporary == 4) { cout << "I am inflexible in my thinking.*"; }
		else if (temporary == 5) { cout << "I am suspicious of strangers and expect the worst of them.*"; }
		else if (temporary == 6) { cout << "Once I pick a goal, I become obsessed with it to the detriment of everything else in my life.*"; }
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 8 && temporary <= 14) { 
		cout << "Charlatan*"; 
		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "I cheat at games of chance.*"; }
		else if (temporary == 2) { cout << "I shave coins or forge documents.*"; }
		else if (temporary == 3) { cout << "I insinuate myself into people's lives to prey on their weakness and secure their fortunes.*"; }
		else if (temporary == 4) { cout << "I put on new identities like clothes.*"; }
		else if (temporary == 5) { cout << "I run sleight-of-hand cons on street corners.*"; }
		else if (temporary == 6) { cout << "I convince people that worthless junk is worth their hard-earned money.*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(8, 1, 1);
		if (temporary == 1) { cout << "I fall in and out of love easily, and am always pursuing someone.*"; }
		else if (temporary == 2) { cout << "I have a joke for every occasion, especially occasions where humor is inappropriate.*"; }
		else if (temporary == 3) { cout << "Flattery is my preferred trick for getting what I want.*"; }
		else if (temporary == 4) { cout << "I'm a born gambler who can't resist taking a risk for a potential payoff.*"; }
		else if (temporary == 5) { cout << "I lie about almost everything, even when there's no good reason to.*"; }
		else if (temporary == 6) { cout << "Sarcasm and insults are my weapons of choice.*"; }
		else if (temporary == 7) { cout << "I keep multiple holy symbols on me and invoke whatever deity might come in useful at any given moment.*"; }
		else if (temporary == 8) { cout << "I pocket anything I see that might have some value.*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "Independence. I am a free spirit— no one tells me what to do.*Chaotic*"; }
		else if (temporary == 2) { cout << "Fairness. I never target people who can't afford to lose a few coins.*Lawful*"; }
		else if (temporary == 3) { cout << "Charity. I distribute the money I acquire to the people who really need it.*Good*"; }
		else if (temporary == 4) { cout << "Creativity. I never run the same con twice.*Chaotic*"; }
		else if (temporary == 5) { cout << "Friendship. Material goods come and go. Bonds of friendship last forever.*Good*"; }
		else if (temporary == 6) { cout << "Aspiration. I'm determined to make something of myself.*Any*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "I fleeced the wrong person and must work to ensure that this individual never crosses paths with me or those I care about.*"; }
		else if (temporary == 2) { cout << "I owe everything to my mentor—a horrible person who's probably rotting in jail somewhere.*"; }
		else if (temporary == 3) { cout << "Somewhere out there, I have a child who doesn't know me. I'm making the world better for him or her.*"; }
		else if (temporary == 4) { cout << "I come from a noble family, and one day I'll reclaim my lands and title from those who stole them from me.*"; }
		else if (temporary == 5) { cout << "A powerful person killed someone I love. Some day soon, I'll have my revenge.*"; }
		else if (temporary == 6) { cout << "I swindled and ruined a person who didn't deserve it. I seek to atone for my misdeeds but might never be able to forgive myself.*"; }
		else { cout << "ERROR*"; }

		temporary = RNG(6, 1, 1);
		if (temporary == 1) { cout << "I can't resist a pretty face.*"; }
		else if (temporary == 2) { cout << "I'm always in debt. I spend my ill-gotten gains on decadent luxuries faster than I bring them in.*"; }
		else if (temporary == 3) { cout << "I'm convinced that no one could ever fool me the way I fool others.*"; }
		else if (temporary == 4) { cout << "I'm too greedy for my own good. I can't resist taking a risk if there's money involved.*"; }
		else if (temporary == 5) { cout << "I can't resist swindling people who are more powerful than me.*"; }
		else if (temporary == 6) { cout << "I hate to admit it and will hate myself for it, but I'll run and preserve my own hide if the going gets tough.*"; }
		else { cout << "ERROR*"; }
	}
	else if (temporary >= 15 && temporary <= 21) { 
	cout << "Criminal*";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "Blackmailer*"; }
	else if (temporary == 2) { cout << "Burglar*"; }
	else if (temporary == 3) { cout << "Enforcer *"; }
	else if (temporary == 4) { cout << "Fence *"; }
	else if (temporary == 5) { cout << "Highway robber*"; }
	else if (temporary == 6) { cout << "Hired killer*"; }
	else if (temporary == 7) { cout << "Pickpocket*"; }
	else if (temporary == 8) { cout << "Smuggler*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I always have a plan for what to do when things go wrong.*"; }
	else if (temporary == 2) { cout << "I am always calm, no matter what the situation. I never raise my voice or let my emptions control me.*"; }
	else if (temporary == 3) { cout << "The first thing to do in a new places is note the locations of everything valuble - or where such things could be hidden.*"; }
	else if (temporary == 4) { cout << "I would rather make a new friend than a new enemy.*"; }
	else if (temporary == 5) { cout << "I am increadibly slow to trust. Those who seem the fairest often have the most to hide.*"; }
	else if (temporary == 6) { cout << "I don't pay attention to the risks in a situation. Never tell me the odds.*"; }
	else if (temporary == 7) { cout << "The best way to get me to do something is to tell me I can't do it.*"; }
	else if (temporary == 8) { cout << "I blow up at the slightest insult.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Honor. I don't steal from others in the trade.*Lawful*"; }
	else if (temporary == 2) { cout << "Freedom. Chains are meant to be broken, as are those who would forge them.*Chaotic*"; }
	else if (temporary == 3) { cout << "Charity. I steal from the wealthy so that I can help people in need.*Good*"; }
	else if (temporary == 4) { cout << "Greed. I will do whatever it takes to become wealthy.*Evil*"; }
	else if (temporary == 5) { cout << "People. I'm loyal to my friends, not to any ideals, and everyone else can take a trip down the Styx for all I care.*Neutral*"; }
	else if (temporary == 6) { cout << "Redemption. There's a spark of good in everyone.*Good*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I'm trying to pay off an old debt I owe to a generous benefactor.*"; }
	else if (temporary == 2) { cout << "My ill-gotten gains go to support my family.*"; }
	else if (temporary == 3) { cout << "Something important was taken from me, and I aim to steal it back.*"; }
	else if (temporary == 4) { cout << "I will become the greatest thief that ever lived.*"; }
	else if (temporary == 5) { cout << "I'm guilty of a terrible crime. I hope I can redeem myself for it.*"; }
	else if (temporary == 6) { cout << "Someone I loved died because of a mistake I made. That will never happen again.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "When I see something valuable, I can't think about anything but how to steal it.*"; }
	else if (temporary == 2) { cout << "When faced with a choice between money and my friends, I usually choose the money.*"; }
	else if (temporary == 3) { cout << "If there's a plan, I'll forget it. If I don't forget it, I'll ignore it.*"; }
	else if (temporary == 4) { cout << "I have a “tell” that reveals when I'm lying.*"; }
	else if (temporary == 5) { cout << "I turn tail and run when things look bad.*"; }
	else if (temporary == 6) { cout << "An innocent person is in prison for a crime that I committed. I'm okay with that.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 22 && temporary <= 28) { 
	cout << "Enterainer*";
	temporary = RNG(10, 1, 1);
	if (temporary == 1) { cout << "Actor *"; }
	else if (temporary == 2) { cout << "Dancer *"; }
	else if (temporary == 3) { cout << "Fire-eater *"; }
	else if (temporary == 4) { cout << "jester *"; }
	else if (temporary == 5) { cout << "juggler *"; }
	else if (temporary == 6) { cout << "Instrumentalist*"; }
	else if (temporary == 7) { cout << "Poet*"; }
	else if (temporary == 8) { cout << "Singer*"; }
	else if (temporary == 9) { cout << "Storyteller*"; }
	else if (temporary == 10) { cout << "Tumbler*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I know a story relevant to almost every situation.*"; }
	else if (temporary == 2) { cout << "Whenever I come to a new place, I collect local rumors and spread gossip.*"; }
	else if (temporary == 3) { cout << "I'm a hopeless romantic, always searching for that “special someone.”*"; }
	else if (temporary == 4) { cout << "Nobody stays angry at me or around me for long, since I can defuse any amount of tension.*"; }
	else if (temporary == 5) { cout << "I love a good insult, even one directed at me.*"; }
	else if (temporary == 6) { cout << "I get bitter if I'm not the center of attention.*"; }
	else if (temporary == 7) { cout << "I'll settle for nothing less than perfection.*"; }
	else if (temporary == 8) { cout << "I change my mood or my mind as quickly as I change key in a song.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Beauty. When I perform, I make the world better than it was.*Good*"; }
	else if (temporary == 2) { cout << "Tradition. The stories, legends, and songs o f the past must never be forgotten, for they teach us who we are.*Lawful*"; }
	else if (temporary == 3) { cout << "Creativity. The world is in need of new ideas and bold action.*Chaotic*"; }
	else if (temporary == 4) { cout << "Greed. I'm only in it for the money and fame.*Evil*"; }
	else if (temporary == 5) { cout << "People. I like seeing the smiles on people's faces when I perform. That's all that matters.*Neutral*"; }
	else if (temporary == 6) { cout << "Honesty. Art should reflect the soul; it should come from within and reveal who we really are.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "My instrument is my most treasured possession, and it reminds me of someone I love.*"; }
	else if (temporary == 2) { cout << "Someone stole my precious instrument, and someday I'll get it back.*"; }
	else if (temporary == 3) { cout << "I want to be famous, whatever it takes.*"; }
	else if (temporary == 4) { cout << "I idolize a hero of the old tales and measure my deeds against that person's.*"; }
	else if (temporary == 5) { cout << "I will do anything to prove myself superior to my hated rival.*"; }
	else if (temporary == 6) { cout << "I would do anything for the other members of my old troupe.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I'll do anything to win fame and renown.*"; }
	else if (temporary == 2) { cout << "I'm a sucker for a pretty face.*"; }
	else if (temporary == 3) { cout << "A scandal prevents me from ever going home again. That kind of trouble seems to follow me around.*"; }
	else if (temporary == 4) { cout << "I once satirized a noble who still wants my head. It was a mistake that I will likely repeat.*"; }
	else if (temporary == 5) { cout << "I have trouble keeping my true feelings hidden. My sharp tongue lands me in trouble.*"; }
	else if (temporary == 6) { cout << "Despite my best efforts, I am unreliable to my friends.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 29 && temporary <= 35) { 
	cout << "Folk Hero*";
	temporary = RNG(10, 1, 1);
	if (temporary == 1) { cout << "I stood up to a tyrant's agents.*"; }
	else if (temporary == 2) { cout << "I saved people during a natural disaster.*"; }
	else if (temporary == 3) { cout << "I stood alone against a terrible monster.*"; }
	else if (temporary == 4) { cout << "I stole from a corrupt merchant to help the poor.*"; }
	else if (temporary == 5) { cout << "I led a militia to fight off an invading army.*"; }
	else if (temporary == 6) { cout << "I broke into a tyrant's castle and stole weapons to arm the people.*"; }
	else if (temporary == 7) { cout << "I trained the peasantry to use farm implements as weapons against a tyrant's soldiers.*"; }
	else if (temporary == 8) { cout << "A lord rescinded an unpopular decree after I led a symbolic act of protect against it.*"; }
	else if (temporary == 9) { cout << "A celestial, fey, or similar creature gave me a blessing or revealed my secret origin.*"; }
	else if (temporary == 10) { cout << "Recruited into a lord's army, I rose to leadership and was commended for my heroism.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I judge people by their actions, not their words.*"; }
	else if (temporary == 2) { cout << "If someone is in trouble, I'm always ready to lend help.*"; }
	else if (temporary == 3) { cout << "When I set my mind to something, I follow through no matter what gets in my way.*"; }
	else if (temporary == 4) { cout << "I have a strong sense of fair play and always try to find the most equitable solution to arguments.*"; }
	else if (temporary == 5) { cout << "I'm confident in my own abilities and do what I can to instill confidence in others.*"; }
	else if (temporary == 6) { cout << "Thinking is for other people. I prefer action.*"; }
	else if (temporary == 7) { cout << "I misuse long words in an attempt to sound smarter.*"; }
	else if (temporary == 8) { cout << "I get bored easily. When am I going to get on with my destiny?*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Respect. People deserve to be treated with dignity and respect.*Good*"; }
	else if (temporary == 2) { cout << "Fairness. No one should get preferential treatment before the law, and no one is above the law.*Lawful*"; }
	else if (temporary == 3) { cout << "Freedom. Tyrants must not be allowed to oppress the people.*Chaotic*"; }
	else if (temporary == 4) { cout << "Might. If I become strong, I can take what I want— what I deserve.*Evil*"; }
	else if (temporary == 5) { cout << "Sincerity. There's no good in pretending to be something I'm not.*Neutral*"; }
	else if (temporary == 6) { cout << "Destiny. Nothing and no one can steer me away from my higher calling.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I have a family, but I have no idea where they are. One day, I hope to see them again.*"; }
	else if (temporary == 2) { cout << "I worked the land, I love the land, and I will protect the land.*"; }
	else if (temporary == 3) { cout << "A proud noble once gave me a horrible beating, and I will take my revenge on any bully I encounter.*"; }
	else if (temporary == 4) { cout << "My tools are symbols of my past life, and I carry them so that I will never forget my roots.*"; }
	else if (temporary == 5) { cout << "I protect those who cannot protect themselves.*"; }
	else if (temporary == 6) { cout << "I wish my childhood sweetheart had come with me to pursue my destiny.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "The tyrant who rules my land will stop at nothing to see me killed.*"; }
	else if (temporary == 2) { cout << "I'm convinced of the significance of my destiny, and blind to my shortcomings and the risk of failure.*"; }
	else if (temporary == 3) { cout << "The people who knew me when I was young know my shameful secret, so I can never go home again.*"; }
	else if (temporary == 4) { cout << "I have a weakness for the vices of the city, especially hard drink.*"; }
	else if (temporary == 5) { cout << "Secretly, I believe that things would be better if I were a tyrant lording over the land.*"; }
	else if (temporary == 6) { cout << "I have trouble trusting in my allies.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 36 && temporary <= 42) { 
	cout << "Guild Artisan*";
	temporary = RNG(20, 1, 1);
	if (temporary == 1) { cout << "Alchemists and apothecaries*"; }
	else if (temporary == 2) { cout << "Armorers, locksmiths, and finesmiths*"; }
	else if (temporary == 3) { cout << "Brewers, distillers, and vintners*"; }
	else if (temporary == 4) { cout << "Calligraphers, scribes, and scriveners*"; }
	else if (temporary == 5) { cout << "Carpenters, roofers, and plasterers*"; }
	else if (temporary == 6) { cout << "Cartographers, surveyors, and chart-makers*"; }
	else if (temporary == 7) { cout << "Cobblers and shoemakers*"; }
	else if (temporary == 8) { cout << "Cooks and bakers*"; }
	else if (temporary == 9) { cout << "Glassblowers and glaziers*"; }
	else if (temporary == 10) { cout << "Jewelers and gemcutters*"; }
	else if (temporary == 11) { cout << "Leatherworkers, skinners, and tanners*"; }
	else if (temporary == 12) { cout << "Masons and stonecutters*"; }
	else if (temporary == 13) { cout << "Painters, limners, and sign-makers*"; }
	else if (temporary == 14) { cout << "Potters and tile-makers*"; }
	else if (temporary == 15) { cout << "Shipwrights and sailmakers*"; }
	else if (temporary == 16) { cout << "Smiths and metal-forgers*"; }
	else if (temporary == 17) { cout << "Tinkers, pewterers, and casters*"; }
	else if (temporary == 18) { cout << "Wagon-makers and wheelwrights*"; }
	else if (temporary == 19) { cout << "Weavers and dyers*"; }
	else if (temporary == 20) { cout << "Woodcarvers, coopers, and bowyers*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I believe that anything worth doing is worth doing right. I can't help it— I'm a perfectionist.*"; }
	else if (temporary == 2) { cout << "I'm a snob who looks down on those who can't appreciate fine art.*"; }
	else if (temporary == 3) { cout << "I always want to know how things work and what makes people tick.*"; }
	else if (temporary == 4) { cout << "I'm full of witty aphorisms and have a proverb for every occasion.*"; }
	else if (temporary == 5) { cout << "I'm rude to people who lack my commitment to hard work and fair play.*"; }
	else if (temporary == 6) { cout << "I like to talk at length about my profession.*"; }
	else if (temporary == 7) { cout << "I don't part with my money easily and will haggle tirelessly to get the best deal possible.*"; }
	else if (temporary == 8) { cout << "I'm well known for my work, and I want to make sure everyone appreciates it. I'm always taken aback when people haven't heard of me.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Community. It is the duty of all civilized people to strengthen the bonds of community and the security of civilization.*Lawful*"; }
	else if (temporary == 2) { cout << "Generosity. My talents were given to me so that I could use them to benefit the world.*Good*"; }
	else if (temporary == 3) { cout << "Freedom. Everyone should be free to pursue his or her own livelihood.*Chaotic*"; }
	else if (temporary == 4) { cout << "Greed. I'm only in it for the money.*Evil*"; }
	else if (temporary == 5) { cout << "People. I'm committed to the people I care about, not to ideals.*Neutral*"; }
	else if (temporary == 6) { cout << "Aspiration. I work hard to be the best there is at my craft.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "The workshop where I learned my trade is the most important place in the world to me.*"; }
	else if (temporary == 2) { cout << "I created a great work for someone, and then found them unworthy to receive it. I'm still looking for someone worthy.*"; }
	else if (temporary == 3) { cout << "I owe my guild a great debt for forging me into the person I am today.*"; }
	else if (temporary == 4) { cout << "I pursue wealth to secure someone's love.*"; }
	else if (temporary == 5) { cout << "One day I will return to my guild and prove that I am the greatest artisan of them all.*"; }
	else if (temporary == 6) { cout << "I will get revenge on the evil forces that destroyed my place of business and ruined my livelihood.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I'll do anything to get my hands on something rare or priceless.*"; }
	else if (temporary == 2) { cout << "I'm quick to assume that someone is trying to cheat me.*"; }
	else if (temporary == 3) { cout << "No one must ever learn that I once stole money from guild coffers.*"; }
	else if (temporary == 4) { cout << "I'm never satisfied with what I have— I always want more.*"; }
	else if (temporary == 5) { cout << "I would kill to acquire a noble title.*"; }
	else if (temporary == 6) { cout << "I'm horribly jealous of anyone who can outshine my handiwork. Everywhere I go, I'm surrounded by rivals.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 43 && temporary <= 49) { 
	cout << "Hermit*";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I was searching for spiritual enlightenment.*"; }
	else if (temporary == 2) { cout << "I was partaking of communal living in accordance with the dictates of a religious order.*"; }
	else if (temporary == 3) { cout << "I was exiled for a crime I didn't commit.*"; }
	else if (temporary == 4) { cout << "I retreated from society after a life-altering event.*"; }
	else if (temporary == 5) { cout << "I needed a quiet place to work on my art, literature, music, or manifesto.*"; }
	else if (temporary == 6) { cout << "I needed to commune with nature, far from civilization.*"; }
	else if (temporary == 7) { cout << "I was the caretaker of an ancient ruin or relic.*"; }
	else if (temporary == 8) { cout << "I was a pilgrim in search of a person, place, or relic of spiritual significance.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I've been isolated for so long that I rarely speak, preferring gestures and the occasional grunt.*"; }
	else if (temporary == 2) { cout << "I am utterly serene, even in the face of disaster.*"; }
	else if (temporary == 3) { cout << "The leader of my community had something wise to say on every topic, and I am eager to share that wisdom.*"; }
	else if (temporary == 4) { cout << "I feel tremendous empathy for all who suffer.*"; }
	else if (temporary == 5) { cout << "I'm oblivious to etiquette and social expectations.*"; }
	else if (temporary == 6) { cout << "I connect everything that happens to me to a grand, cosmic plan.*"; }
	else if (temporary == 7) { cout << "I often get lost in my own thoughts and contemplation, becoming oblivious to my surroundings.*"; }
	else if (temporary == 8) { cout << "I am working on a grand philosophical theory and love sharing my ideas.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Greater Good. My gifts are meant to be shared with all, not used for my own benefit.*Good*"; }
	else if (temporary == 2) { cout << "Logic. Emotions must not cloud our sense of what is right and true, or our logical thinking.*Lawful*"; }
	else if (temporary == 3) { cout << "Free Thinking. Inquiry and curiosity are the pillars of progress.*Chaotic*"; }
	else if (temporary == 4) { cout << "Power. Solitude and contemplation are paths toward mystical or magical power.*Evil*"; }
	else if (temporary == 5) { cout << "Live and Let Live. Meddling in the affairs o f others only causes trouble.*Neutral*"; }
	else if (temporary == 6) { cout << "Self-Knowledge. If you know yourself, there's nothing left to know.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Nothing is more important than the other members of my hermitage, order, or association.*"; }
	else if (temporary == 2) { cout << "I entered seclusion to hide from the ones who might still be hunting me. I must someday confront them.*"; }
	else if (temporary == 3) { cout << "I'm still seeking the enlightenment I pursued in my seclusion, and it still eludes me.*"; }
	else if (temporary == 4) { cout << "I entered seclusion because I loved someone I could not have.*"; }
	else if (temporary == 5) { cout << "Should my discovery come to light, it could bring ruin to the world.*"; }
	else if (temporary == 6) { cout << "My isolation gave me great insight into a great evil that only I can destroy.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Now that I've returned to the world, I enjoy its delights a little too much.*"; }
	else if (temporary == 2) { cout << "I harbor dark, bloodthirsty thoughts that my isolation and meditation failed to quell.*"; }
	else if (temporary == 3) { cout << "I am dogmatic in my thoughts and philosophy.*"; }
	else if (temporary == 4) { cout << "I let my need to win arguments overshadow friendships and harmony.*"; }
	else if (temporary == 5) { cout << "I'd risk too much to uncover a lost bit of knowledge.*"; }
	else if (temporary == 6) { cout << "I like keeping secrets and won't share them with anyone.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 50 && temporary <= 56) { 
	cout << "Noble* *";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "My eloquent flattery makes everyone I talk to feel like the most wonderful and important person in the world.*"; }
	else if (temporary == 2) { cout << "The common folk love me for my kindness and generosity.*"; }
	else if (temporary == 3) { cout << "No one could doubt by looking at my regal bearing that I am a cut above the unwashed masses.*"; }
	else if (temporary == 4) { cout << "I take great pains to always look my best and follow the latest fashions.*"; }
	else if (temporary == 5) { cout << "I don't like to get my hands dirty, and I won't be caught dead in unsuitable accommodations.*"; }
	else if (temporary == 6) { cout << "Despite my noble birth, I do not place myself above other folk. We all have the same blood.*"; }
	else if (temporary == 7) { cout << "My favor, once lost, is lost forever.*"; }
	else if (temporary == 8) { cout << "If you do me an injury, I will crush you, ruin your name, and salt your fields.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Respect. Respect is due to me because of my position, but all people regardless of station deserve to be treated with dignity.*Good*"; }
	else if (temporary == 2) { cout << "Responsibility. It is my duty to respect the authority of those above me, just as those below me must respect mine.*Lawful*"; }
	else if (temporary == 3) { cout << "Independence. I must prove that I can handle myself without the coddling of my family.*Chaotic*"; }
	else if (temporary == 4) { cout << "Power. If I can attain more power, no one will tell me what to do.*Evil*"; }
	else if (temporary == 5) { cout << "Family. Blood runs thicker than water.*Any*"; }
	else if (temporary == 6) { cout << "Noble Obligation. It is my duty to protect and care for the people beneath me.*Good*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I will face any challenge to win the approval of my family.*"; }
	else if (temporary == 2) { cout << "My house's alliance with another noble family must be sustained at all costs.*"; }
	else if (temporary == 3) { cout << "Nothing is more important than the other members of my family.*"; }
	else if (temporary == 4) { cout << "I am in love with the heir of a family that my family despises.*"; }
	else if (temporary == 5) { cout << "My loyalty to my sovereign is unwavering.*"; }
	else if (temporary == 6) { cout << "The common folk must see me as a hero o f the people.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I secretly believe that everyone is beneath me.*"; }
	else if (temporary == 2) { cout << "I hide a truly scandalous secret that could ruin my family forever.*"; }
	else if (temporary == 3) { cout << "I too often hear veiled insults and threats in every word addressed to me, and I'm quick to anger.*"; }
	else if (temporary == 4) { cout << "I have an insatiable desire for carnal pleasures.*"; }
	else if (temporary == 5) { cout << "In fact, the world does revolve around me.*"; }
	else if (temporary == 6) { cout << "By my words and actions, I often bring shame to my family.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 57 && temporary <= 63) { 
	cout << "Outlander*";
	temporary = RNG(10, 1, 1);
	if (temporary == 1) { cout << "Forester *"; }
	else if (temporary == 2) { cout << "Trapper *"; }
	else if (temporary == 3) { cout << "Homesteader *"; }
	else if (temporary == 4) { cout << "Guide *"; }
	else if (temporary == 5) { cout << "Exile or outcast *"; }
	else if (temporary == 6) { cout << "Bounty hunter*"; }
	else if (temporary == 7) { cout << "Pilgrim*"; }
	else if (temporary == 8) { cout << "Tribal nomad*"; }
	else if (temporary == 9) { cout << "Hunter-gatherer*"; }
	else if (temporary == 10) { cout << "Tribal marauder*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I'm driven by a wanderlust that led me away from home.*"; }
	else if (temporary == 2) { cout << "I watch over my friends as if they were a litter of newborn pups.*"; }
	else if (temporary == 3) { cout << "I once ran twenty-five miles without stopping to warn to my clan of an approaching orc horde. I'd do it again if I had to.*"; }
	else if (temporary == 4) { cout << "I have a lesson for every situation, drawn from observing nature.*"; }
	else if (temporary == 5) { cout << "I place no stock in wealthy or well-mannered folk. Money and manners won't save you from a hungry owlbear.*"; }
	else if (temporary == 6) { cout << "I'm always picking things up, absently fiddling with them, and sometimes accidentally breaking them.*"; }
	else if (temporary == 7) { cout << "I feel far more comfortable around animals than people.*"; }
	else if (temporary == 8) { cout << "I was, in fact, raised by wolves.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Change. Life is like the seasons, in constant change, and we must change with it.*Chaotic*"; }
	else if (temporary == 2) { cout << "Greater Good. It is each person's responsibility to make the most happiness for the whole tribe.*Good*"; }
	else if (temporary == 3) { cout << "Honor. If I dishonor myself, I dishonor my whole clan.*Lawful*"; }
	else if (temporary == 4) { cout << "Might. The strongest are meant to rule.*Evil*"; }
	else if (temporary == 5) { cout << "Nature. The natural world is more important than all the constructs of civilization.*Neutral*"; }
	else if (temporary == 6) { cout << "Glory. I must earn glory in battle, for myself and my clan.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "My family, clan, or tribe is the most important thing in my life, even when they are far from me.*"; }
	else if (temporary == 2) { cout << "An injury to the unspoiled wilderness of my home is an injury to me.*"; }
	else if (temporary == 3) { cout << "I will bring terrible wrath down on the evildoers who destroyed my homeland.*"; }
	else if (temporary == 4) { cout << "I am the last of my tribe, and it is up to me to ensure their names enter legend.*"; }
	else if (temporary == 5) { cout << "I suffer awful visions of a coming disaster and will do anything to prevent it.*"; }
	else if (temporary == 6) { cout << "It is my duty to provide children to sustain my tribe.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I am too enamored of ale, wine, and other intoxicants.*"; }
	else if (temporary == 2) { cout << "There's no room for caution in a life lived to the fullest.*"; }
	else if (temporary == 3) { cout << "I remember every insult I've received and nurse a silent resentment toward anyone who's ever wronged me.*"; }
	else if (temporary == 4) { cout << "I am slow to trust members of other races, tribes, and societies.*"; }
	else if (temporary == 5) { cout << "Violence is my answer to almost any challenge.*"; }
	else if (temporary == 6) { cout << "Don't expect me to save those who can't save themselves. It is nature's way that the strong thrive and the weak perish.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 64 && temporary <= 70) { 
	cout << "Sage*";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "Alchemist *"; }
	else if (temporary == 2) { cout << "Astronomer *"; }
	else if (temporary == 3) { cout << "Discredited academic*"; }
	else if (temporary == 4) { cout << "Librarian*"; }
	else if (temporary == 5) { cout << "Professor*"; }
	else if (temporary == 6) { cout << "Researcher*"; }
	else if (temporary == 7) { cout << "Wizard's apprentice*"; }
	else if (temporary == 8) { cout << "Scribe*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I use polysyllabic words that convey the impression of great erudition.*"; }
	else if (temporary == 2) { cout << "I've read every book in the world's greatest libraries— or I like to boast that I have.*"; }
	else if (temporary == 3) { cout << "I'm used to helping out those who aren't as smart as I am, and I patiently explain anything and everything to others.*"; }
	else if (temporary == 4) { cout << "There's nothing I like more than a good mystery.*"; }
	else if (temporary == 5) { cout << "I'm willing to listen to every side of an argument before I make my own judgment.*"; }
	else if (temporary == 6) { cout << "I . . . speak . . . slowly . . . when talking . . . to idiots, . . . which . . . almost . . . everyone . . . is . . . compared . . . to me.*"; }
	else if (temporary == 7) { cout << "I am horribly, horribly awkward in social situations.*"; }
	else if (temporary == 8) { cout << "I'm convinced that people are always trying to steal my secrets.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Knowledge. The path to power and self-improvement is through knowledge.*Neutral*"; }
	else if (temporary == 2) { cout << "Beauty. What is beautiful points us beyond itself toward what is true.*Good*"; }
	else if (temporary == 3) { cout << "Logic. Emotions must not cloud our logical thinking.*Lawful*"; }
	else if (temporary == 4) { cout << "No Limits. Nothing should fetter the infinite possibility inherent in all existence.*Chaotic*"; }
	else if (temporary == 5) { cout << "Power. Knowledge is the path to power and domination.*Evil*"; }
	else if (temporary == 6) { cout << "Self-Improvement. The goal of a life of study is the betterment of oneself.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "It is my duty to protect my students.*"; }
	else if (temporary == 2) { cout << "I have an ancient text that holds terrible secrets that must not fall into the wrong hands.*"; }
	else if (temporary == 3) { cout << "I work to preserve a library, university, scriptorium, or monastery.*"; }
	else if (temporary == 4) { cout << "My life's work is a series o f tomes related to a specific field of lore.*"; }
	else if (temporary == 5) { cout << "I've been searching my whole life for the answer to a certain question.*"; }
	else if (temporary == 6) { cout << "I sold my soul for knowledge. I hope to do great deeds and win it back.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I am easily distracted by the promise of information.*"; }
	else if (temporary == 2) { cout << "Most people scream and run when they see a demon. I stop and take notes on its anatomy.*"; }
	else if (temporary == 3) { cout << "Unlocking an ancient mystery is worth the price of a civilization.*"; }
	else if (temporary == 4) { cout << "I overlook obvious solutions in favor of complicated ones.*"; }
	else if (temporary == 5) { cout << "I speak without really thinking through my words, invariably insulting others.*"; }
	else if (temporary == 6) { cout << "I can't keep a secret to save my life, or anyone else's.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 71 && temporary <= 77) { 
	cout << "Salor* *";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "My friends know they can rely on me, no matter what.*"; }
	else if (temporary == 2) { cout << "I work hard so that I can play hard when the work is done.*"; }
	else if (temporary == 3) { cout << "I enjoy sailing into new ports and making new friends over a flagon of ale.*"; }
	else if (temporary == 4) { cout << "I stretch the truth for the sake of a good story.*"; }
	else if (temporary == 5) { cout << "To me, a tavern brawl is a nice way to get to know a new city.*"; }
	else if (temporary == 6) { cout << "I never pass up a friendly wager.*"; }
	else if (temporary == 7) { cout << "My language is as foul as an otyugh nest.*"; }
	else if (temporary == 8) { cout << "I like a job well done, especially if I can convince someone else to do it.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Respect. The thing that keeps a ship together is mutual respect between captain and crew.*Good*"; }
	else if (temporary == 2) { cout << "Fairness. We all do the work, so we all share in the rewards.*Lawful*"; }
	else if (temporary == 3) { cout << "Freedom. The sea is freedom—the freedom to go anywhere and do anything.*Chaotic*"; }
	else if (temporary == 4) { cout << "Mastery. I'm a predator, and the other ships on the sea are my prey.*Evil*"; }
	else if (temporary == 5) { cout << "People. I'm committed to my crewmates, not to ideals.*Neutral*"; }
	else if (temporary == 6) { cout << "Aspiration. Someday I'll own my own ship and chart my own destiny.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I'm loyal to my captain first, everything else second.*"; }
	else if (temporary == 2) { cout << "The ship is most important—crewmates and captains come and go.*"; }
	else if (temporary == 3) { cout << "I'll always remember my first ship.*"; }
	else if (temporary == 4) { cout << "In a harbor town, I have a paramour whose eyes nearly stole me from the sea.*"; }
	else if (temporary == 5) { cout << "I was cheated out of my fair share of the profits, and I want to get my due.*"; }
	else if (temporary == 6) { cout << "Ruthless pirates murdered my captain and crewmates, plundered our ship, and left me to die. Vengeance will be mine.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I follow orders, even if I think they're wrong.*"; }
	else if (temporary == 2) { cout << "I'll say anything to avoid having to do extra work.*"; }
	else if (temporary == 3) { cout << "Once someone questions my courage, I never back down no matter how dangerous the situation.*"; }
	else if (temporary == 4) { cout << "Once I start drinking, it's hard for me to stop.*"; }
	else if (temporary == 5) { cout << "I can't help but pocket loose coins and other trinkets I come across.*"; }
	else if (temporary == 6) { cout << "My pride will probably lead to my destruction.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 78 && temporary <= 84) { 
	cout << "Soilder*";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "Officer *"; }
	else if (temporary == 2) { cout << "Scout *"; }
	else if (temporary == 3) { cout << "Infantry *"; }
	else if (temporary == 4) { cout << "Cavalry*"; }
	else if (temporary == 5) { cout << "Healer*"; }
	else if (temporary == 6) { cout << "Quartermaster*"; }
	else if (temporary == 7) { cout << "Standard bearer*"; }
	else if (temporary == 8) { cout << "Support staff, cook, blacksmith, or the like*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I'm always polite and respectful.*"; }
	else if (temporary == 2) { cout << "I'm haunted by memories o f war. I can't get the images of violence out of my mind.*"; }
	else if (temporary == 3) { cout << "I've lost too many friends, and I'm slow to make new ones.*"; }
	else if (temporary == 4) { cout << "I'm full of inspiring and cautionary tales from my military experience relevant to almost every combat situation.*"; }
	else if (temporary == 5) { cout << "I can stare down a hell hound without flinching.*"; }
	else if (temporary == 6) { cout << "I enjoy being strong and like breaking things.*"; }
	else if (temporary == 7) { cout << "I have a crude sense of humor.*"; }
	else if (temporary == 8) { cout << "I face problems head-on. A simple, direct solution is the best path to success.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Greater Good. Our lot is to lay down our lives in defense of others.*Good*"; }
	else if (temporary == 2) { cout << "Responsibility. I do what I must and obey just authority.*Lawful*"; }
	else if (temporary == 3) { cout << "Independence. When people follow orders blindly, they embrace a kind of tyranny.*Chaotic*"; }
	else if (temporary == 4) { cout << "Might. In life as in war, the stronger force wins.*Evil*"; }
	else if (temporary == 5) { cout << "Live and Let Live. Ideals aren't worth killing over or going to war for.*Neutral*"; }
	else if (temporary == 6) { cout << "Nation. My city, nation, or people are all that matter.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "I would still lay down my life for the people I served with.*"; }
	else if (temporary == 2) { cout << "Someone saved my life on the battlefield. To this day, I will never leave a friend behind.*"; }
	else if (temporary == 3) { cout << "My honor is my life.*"; }
	else if (temporary == 4) { cout << "I'll never forget the crushing defeat my company suffered or the enemies who dealt it.*"; }
	else if (temporary == 5) { cout << "Those who fight beside me are those worth dying for.*"; }
	else if (temporary == 6) { cout << "I fight for those who cannot fight for themselves.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "The monstrous enemy we faced in battle still leaves me quivering with fear.*"; }
	else if (temporary == 2) { cout << "I have little respect for anyone who is not a proven warrior.*"; }
	else if (temporary == 3) { cout << "I made a terrible mistake in battle cost many lives— and I would do anything to keep that mistake secret.*"; }
	else if (temporary == 4) { cout << "My hatred of my enemies is blind and unreasoning.*"; }
	else if (temporary == 5) { cout << "I obey the law, even if the law causes misery.*"; }
	else if (temporary == 6) { cout << "I'd rather eat my armor than admit when I'm wrong.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 85 && temporary <= 91) { 
	cout << "Urchin* *";
	temporary = RNG(8, 1, 1);
	if (temporary == 1) { cout << "I hide scraps of food and trinkets away in my pockets.*"; }
	else if (temporary == 2) { cout << "I ask a lot of questions.*"; }
	else if (temporary == 3) { cout << "I like to squeeze into small places where no one else can get to me.*"; }
	else if (temporary == 4) { cout << "I sleep with my back to a wall or tree, with everything I own wrapped in a bundle in my arms.*"; }
	else if (temporary == 5) { cout << "I eat like a pig and have bad manners.*"; }
	else if (temporary == 6) { cout << "I think anyone who's nice to me is hiding evil intent.*"; }
	else if (temporary == 7) { cout << "I don't like to bathe.*"; }
	else if (temporary == 8) { cout << "I bluntly say what other people are hinting at or hiding.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "Respect. All people, rich or poor, deserve respect.*Good*"; }
	else if (temporary == 2) { cout << "Community. We have to take care of each other, because no one else is going to do it.*Lawful*"; }
	else if (temporary == 3) { cout << "Change. The low are lifted up, and the high and mighty are brought down. Change is the nature of things.*Chaotic*"; }
	else if (temporary == 4) { cout << "Retribution. The rich need to be shown what life and death are like in the gutters.*Evil*"; }
	else if (temporary == 5) { cout << "People. I help the people who help me—that's what keeps us alive.*Neutral*"; }
	else if (temporary == 6) { cout << "Aspiration. I'm going to prove that I'm worthy of a better life.*Any*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "My town or city is my home, and I'll fight to defend it.*"; }
	else if (temporary == 2) { cout << "I sponsor an orphanage to keep others from enduring what I was forced to endure.*"; }
	else if (temporary == 3) { cout << "I owe my survival to another urchin who taught me to live on the streets.*"; }
	else if (temporary == 4) { cout << "I owe a debt I can never repay to the person who took pity on me.*"; }
	else if (temporary == 5) { cout << "I escaped my life of poverty by robbing an important person, and I'm wanted for it.*"; }
	else if (temporary == 6) { cout << "No one else should have to endure the hardships I've been through.*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(6, 1, 1);
	if (temporary == 1) { cout << "If I'm outnumbered, I will run away from a fight.*"; }
	else if (temporary == 2) { cout << "Gold seems like a lot of money to me, and I'll do just about anything for more of it.*"; }
	else if (temporary == 3) { cout << "I will never fully trust anyone other than myself.*"; }
	else if (temporary == 4) { cout << "I'd rather kill someone in their sleep then fight fair.*"; }
	else if (temporary == 5) { cout << "It's not stealing if I need it more than someone else.*"; }
	else if (temporary == 6) { cout << "People who can't take care of themselves get what they deserve.*"; }
	else { cout << "ERROR*"; }
	}
	else if (temporary >= 92 && temporary <= 100) {cout << "Choose Background* * * * * * *";}
	else { cout << "ERROR*"; }

	return 0; 
} // Add Sub stuff

int CharacterClass(int n) {
	int temporary = 0;

	temporary = RNG(100, 1, 1);
	if (temporary >= 1 && temporary <= 8) { cout << "Barbarian* *"; n = RNG(4, 1, 2); cout << n; cout << "0*"; }
	else if (temporary >= 9 && temporary <= 16) { cout << "Bard* *"; n = RNG(4, 1, 5); cout << n; cout << "0*";}
	else if (temporary >= 17 && temporary <= 24) { cout << "Cleric*"; 
		n =	RNG(10, 1, 1);
		if (n == 1) { cout << "Forge*"; }
		else if (n == 2) { cout << "Grave*"; }
		else if (n == 3) { cout << "Knowledge*"; }
		else if (n == 4) { cout << "Life*"; }
		else if (n == 5) { cout << "Light*"; }
		else if (n == 6) { cout << "Nature*"; }
		else if (n == 7) { cout << "Tempest*"; }
		else if (n == 8) { cout << "Trickery*"; }
		else if (n == 9) { cout << "War*"; }
		else if (n == 10) { cout << "Choose*"; }
		else { cout << "ERROR*"; }
		n = RNG(4, 1, 5); cout << n; cout << "0*";
	}
	else if (temporary >= 25 && temporary <= 32) { cout << "Druid* *"; n = RNG(4, 1, 2); cout << n; cout << "0*";}
	else if (temporary >= 33 && temporary <= 40) { cout << "Fighter*"; 
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 16) { cout << "Archery*"; }
		else if (n >= 17 && n <= 32) { cout << "Defense*"; }
		else if (n >= 33 && n <= 48) { cout << "Dueling*"; }
		else if (n >= 49 && n <= 64) { cout << "Great Weapon*"; }
		else if (n >= 65 && n <= 80) { cout << "Protection*"; }
		else if (n >= 81 && n <= 96) { cout << "Two-Weapon*"; }
		else if (n >= 97 && n <= 100) { cout << "Choose*"; }
		else { cout << "ERROR*"; }
		n = RNG(4, 1, 5); cout << n; cout << "0*";
	}
	else if (temporary >= 41 && temporary <= 48) { cout << "Monk* *"; n = RNG(4, 1, 5); cout << n; cout << "*";}
	else if (temporary >= 49 && temporary <= 56) { cout << "Paladin* *"; n = RNG(4, 1, 5); cout << n; cout << "0*";}
	else if (temporary >= 57 && temporary <= 64) { cout << "Ranger* *"; n = RNG(4, 1, 5); cout << n; cout << "0*";}
	else if (temporary >= 65 && temporary <= 72) { cout << "Rogue* *"; n = RNG(4, 1, 4); cout << n; cout << "0*";}
	else if (temporary >= 73 && temporary <= 80) { cout << "Socerer*"; 
		n = RNG(100, 1, 1);
		if (n >= 1 && n <= 19) { cout << "Divine Soul*"; }
		else if (n >= 20 && n <= 38) { cout << "Draconic Bloodline*"; }
		else if (n >= 39 && n <= 57) { cout << "Shadow Magic*"; }
		else if (n >= 58 && n <= 76) { cout << "Storm Socery*"; }
		else if (n >= 77 && n <= 95) { cout << "Wild Magic*"; }
		else if (n >= 96 && n <= 100) { cout << "Choose*"; }
		else { cout << "ERROR*"; }
		n = RNG(4, 1, 3); cout << n; cout << "0*";
	}
	else if (temporary >= 81 && temporary <= 88) { cout << "Warlock*"; 
	n = RNG(100, 1, 1);
		if (n >= 1 && n <= 19) { cout << "Archfey*"; }
		else if (n >= 20 && n <= 38) { cout << "Celestial*"; }
		else if (n >= 39 && n <= 57) { cout << "Fiend*"; }
		else if (n >= 58 && n <= 76) { cout << "Great Old One*"; }
		else if (n >= 77 && n <= 95) { cout << "Hexblade*"; }
		else if (n >= 96 && n <= 100) { cout << "Choose*"; }
		else { cout << "ERROR*"; }
		n = RNG(4, 1, 4); cout << n; cout << "0*";
	}
	else if (temporary >= 89 && temporary <= 96) { cout << "Wizard* *"; n = RNG(4, 1, 4); cout << n; cout << "0*";}
	else if (temporary >= 97 && temporary <= 100) { cout << "Choose* * *"; }
	else { cout << "ERROR*"; }

	return 0;
}

int CharacterStats(int str, int dex, int con, int inl, int wis, int chr) {
	int temporary = 0;

	temporary = RNG(6, 1, 3);
	temporary = temporary + str;
	cout << temporary;
	cout << "*";
	temporary = RNG(6, 1, 3);
	temporary = temporary + dex;
	cout << temporary;
	cout << "*";
	temporary = RNG(6, 1, 3);
	temporary = temporary + con;
	cout << temporary;
	cout << "*";
	temporary = RNG(6, 1, 3);
	temporary = temporary + inl;
	cout << temporary;
	cout << "*";
	temporary = RNG(6, 1, 3);
	temporary = temporary + wis;
	cout << temporary;
	cout << "*";
	temporary = RNG(6, 1, 3);
	temporary = temporary + chr;
	cout << temporary;
	cout << "*";
	return 0;
}

int CharacterAppearance(int n) {
	n = RNG(100, 1, 1);
	if (n >= 1 && n <= 39) { cout << "Young Adult*"; }
	else if (n >= 40 && n <= 74) { cout << "Early middle-age*"; }
	else if (n >= 75 && n <= 91) { cout << "Late middle-age*"; }
	else if (n >= 92 && n <= 97) { cout << "Old*"; }
	else if (n >= 98 && n <= 100) { cout << "Very old*"; }
	else { cout << "ERROR*"; }

	n = RNG(100, 1, 1);
	if (n >= 1 && n <= 5) { cout << "Very Thin*"; }
	else if (n >= 6 && n <= 30) { cout << "Thin*"; }
	else if (n >= 31 && n <= 70) { cout << "Average*"; }
	else if (n >= 71 && n <= 95) { cout << "Fat*"; }
	else if (n >= 96 && n <= 100) { cout << "Very Fat*"; }
	else { cout << "ERROR*"; }

	n = RNG(100, 1, 1);
	if (n >= 1 && n <= 5) { cout << "Very Short*"; }
	else if (n >= 6 && n <= 30) { cout << "Short*"; }
	else if (n >= 31 && n <= 70) { cout << "Average*"; }
	else if (n >= 71 && n <= 95) { cout << "Tall*"; }
	else if (n >= 96 && n <= 100) { cout << "Very Tall*"; }
	else { cout << "ERROR*"; }

	n = RNG(100, 1, 1);
	if (n >= 1 && n <= 20) { cout << "Scar*"; }
	else if (n >= 21 && n <= 40) { cout << "Tattoo*"; }
	else if (n >= 41 && n <= 60) { cout << "Piercing*"; }
	else if (n >= 61 && n <= 80) { cout << "Birthmark*"; }
	else if (n >= 81 && n <= 100) { cout << "Accent*"; }
	else { cout << "ERROR*"; }

	return 0;
}

int CharacterMotivationAndHabits(int n) {
	int temporary = 0;

	temporary = RNG(100, 1, 1);
	if (temporary >= 1 && temporary <= 6) { cout << "Acheivement*"; }
	else if (temporary >= 7 && temporary <= 12) { cout << "Acquisition*"; }
	else if (temporary >= 13 && temporary <= 18) { cout << "Balance*"; }
	else if (temporary >= 19 && temporary <= 24) { cout << "Beneficence*"; }
	else if (temporary >= 25 && temporary <= 30) { cout << "Creation*"; }
	else if (temporary >= 31 && temporary <= 36) { cout << "Discovery*"; }
	else if (temporary >= 37 && temporary <= 42) { cout << "Education*"; }
	else if (temporary >= 43 && temporary <= 48) { cout << "Hedonism*"; }
	else if (temporary >= 49 && temporary <= 54) { cout << "Liberation*"; }
	else if (temporary >= 55 && temporary <= 60) { cout << "Nobility*";	}
	else if (temporary >= 61 && temporary <= 66) { cout << "Order*"; }
	else if (temporary >= 67 && temporary <= 73) { cout << "Play*"; }
	else if (temporary >= 74 && temporary <= 79) { cout << "Power*"; }
	else if (temporary >= 80 && temporary <= 85) { cout << "Recognition*"; }
	else if (temporary >= 86 && temporary <= 91) { cout << "Service*"; }
	else if (temporary >= 92 && temporary <= 97) { cout << "Understanding*"; }
	else if (temporary >= 98 && temporary <= 100) { cout << "Choose*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(100, 1, 1);
	if (temporary >= 1 && temporary <= 3) { cout << "Humming*"; }
	else if (temporary >= 4 && temporary <= 6) { cout << "Dancing*"; }
	else if (temporary >= 7 && temporary <= 9) { cout << "Sleepwalking*"; }
	else if (temporary >= 10 && temporary <= 12) { cout << "Facisl Tics*"; }
	else if (temporary >= 13 && temporary <= 15) { cout << "Fingernail Biting*"; }
	else if (temporary >= 16 && temporary <= 18) { cout << "Daydreaming*"; }
	else if (temporary >= 19 && temporary <= 21) { cout << "Talking in Sleep*"; }
	else if (temporary >= 22 && temporary <= 24) { cout << "Whistling*"; }
	else if (temporary >= 25 && temporary <= 27) { cout << "Name Dropping*"; }
	else if (temporary >= 28 && temporary <= 30) { cout << "Constant Grooming*"; }
	else if (temporary >= 31 && temporary <= 33) { cout << "Foot Tapping*"; }
	else if (temporary >= 34 && temporary <= 36) { cout << "Lip Bitting/Licking*"; }
	else if (temporary >= 37 && temporary <= 39) { cout << "Coin Flipping*"; }
	else if (temporary >= 40 && temporary <= 42) { cout << "Chewing*"; }
	else if (temporary >= 43 && temporary <= 45) { cout << "Knuckle Cracking*"; }
	else if (temporary >= 46 && temporary <= 48) { cout << "Collects odd Things*"; }
	else if (temporary >= 49 && temporary <= 51) { cout << "Singing*"; }
	else if (temporary >= 52 && temporary <= 54) { cout << "Snacking*"; }
	else if (temporary >= 55 && temporary <= 57) { cout << "Pacing*"; }
	else if (temporary >= 58 && temporary <= 60) { cout << "Counting*"; }
	else if (temporary >= 61 && temporary <= 63) { cout << "Snoring*"; }
	else if (temporary >= 64 && temporary <= 66) { cout << "Beard/hair Strocking*"; }
	else if (temporary >= 67 && temporary <= 69) { cout << "Nose Picking*"; }
	else if (temporary >= 70 && temporary <= 72) { cout << "Apologizing*"; }
	else if (temporary >= 73 && temporary <= 75) { cout << "Exaggeration*"; }
	else if (temporary >= 76 && temporary <= 78) { cout << "Supersitious*"; }
	else if (temporary >= 79 && temporary <= 81) { cout << "Belching*"; }
	else if (temporary >= 82 && temporary <= 84) { cout << "Repeating Others*"; }
	else if (temporary >= 85 && temporary <= 87) { cout << "Smelling Things*"; }
	else if (temporary >= 88 && temporary <= 90) { cout << "Teeth Picking*"; }
	else if (temporary >= 91 && temporary <= 93) { cout << "Swearing*"; }
	else if (temporary >= 94 && temporary <= 96) { cout << "Telling Stories*"; }
	else if (temporary >= 97 && temporary <= 99) { cout << "Repeating Yourself*"; }
	else if (temporary == 100) { cout << "Choose*"; }
	else { cout << "ERROR*"; }

	temporary = RNG(100, 1, 1);
	if (temporary >= 1 && temporary <= 10) { cout << "Acquire*"; }
	else if (temporary >= 11 && temporary <= 20) { cout << "Craft*"; }
	else if (temporary >= 21 && temporary <= 30) { cout << "Deliver*"; }
	else if (temporary >= 31 && temporary <= 40) { cout << "Destroy*"; }
	else if (temporary >= 41 && temporary <= 50) { cout << "Discover*"; }
	else if (temporary >= 51 && temporary <= 60) { cout << "Explore*"; }
	else if (temporary >= 61 && temporary <= 70) { cout << "Justice*"; }
	else if (temporary >= 71 && temporary <= 80) { cout << "Learn*"; }
	else if (temporary >= 81 && temporary <= 90) { cout << "Meet*"; }
	else if (temporary >= 91 && temporary <= 100) { cout << "Vengeance*"; }
	else { cout << "ERROR*"; }

	return 0;
}

int DarkerDungeon() {
	int temporary = 0;
	int Repeat = 0;
	int temp = 0;
	
	cout << "\nHow many Characters need to be Made? ";
	cin >> Repeat;
	srand(time(NULL));
	temp = rand();
	PlayerID = (temp) % 9999 + 1;
	cout << "Remember to Reroll one Stat and swap two!!!\nTo break this down, port the data over to excel and delimit(Text to Column) the data by *. \n\n";

	for (int i = 0; i < Repeat; i++) {
		temp = rand();
		CharacterID = (temp) % 9999 + 1;
		temporary = CharacterRace(temporary); // Will print out and reset the temporary variable.
		temporary = CharacterBackground(temporary); // Will print out and reset the temporary variable.
		temporary = CharacterClass(temporary); // Will print out and reset the temporary variable.
		temporary = CharacterStats(0, 0, 0, 0, 0, 0);
		temporary = CharacterAppearance(temporary);
		temporary = CharacterMotivationAndHabits(temporary);

		cout << "\n";

		/*temp = RNG(100, 1, 1);
		cout << temp;
		cout << " ";*/

	}

	//cout << "\n";	
	return 0;
}