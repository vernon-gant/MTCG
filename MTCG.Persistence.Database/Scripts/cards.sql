DELETE
FROM Cards;
DELETE
FROM Elements;

INSERT INTO Elements (Name)
VALUES ('Fire');
INSERT INTO Elements (Name)
VALUES ('Water');
INSERT INTO Elements (Name)
VALUES ('Earth');
INSERT INTO Elements (Name)
VALUES ('Air');
INSERT INTO Elements (Name)
VALUES ('Shadow');
INSERT INTO Elements (Name)
VALUES ('Normal');

INSERT INTO Cards (Name, ElementId)
VALUES ('Goblin', (SELECT ElementId FROM Elements WHERE Name = 'Fire'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Dragon', (SELECT ElementId FROM Elements WHERE Name = 'Fire'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Wizard', (SELECT ElementId FROM Elements WHERE Name = 'Fire'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Ork', (SELECT ElementId FROM Elements WHERE Name = 'Normal'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Knight', (SELECT ElementId FROM Elements WHERE Name = 'Normal'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Kraken', (SELECT ElementId FROM Elements WHERE Name = 'Water'));
INSERT INTO Cards (Name, ElementId)
VALUES ('FireElf', (SELECT ElementId FROM Elements WHERE Name = 'Fire'));


INSERT INTO Cards (Name, ElementId)
VALUES ('FlameStrike', (SELECT ElementId FROM Elements WHERE Name = 'Fire'));
INSERT INTO Cards (Name, ElementId)
VALUES ('WaterBlast', (SELECT ElementId FROM Elements WHERE Name = 'Water'));
INSERT INTO Cards (Name, ElementId)
VALUES ('EarthShatter', (SELECT ElementId FROM Elements WHERE Name = 'Earth'));
INSERT INTO Cards (Name, ElementId)
VALUES ('AirSlice', (SELECT ElementId FROM Elements WHERE Name = 'Air'));
INSERT INTO Cards (Name, ElementId)
VALUES ('ShadowFog', (SELECT ElementId FROM Elements WHERE Name = 'Shadow'));
INSERT INTO Cards (Name, ElementId)
VALUES ('Heal', (SELECT ElementId FROM Elements WHERE Name = 'Normal'));


