DROP TABLE IF EXISTS BattleDecks;
DROP TABLE IF EXISTS ActiveDecks;
DROP TABLE IF EXISTS Battles;
DROP TABLE IF EXISTS Decks;
DROP TABLE IF EXISTS UserCards;
DROP TABLE IF EXISTS CardPackageContents;
DROP TABLE IF EXISTS CardPackages;
DROP TABLE IF EXISTS Deals;
DROP TABLE IF EXISTS Cards;
DROP TABLE IF EXISTS Elements;
DROP TABLE IF EXISTS Users;


DROP TYPE IF EXISTS battle_result;

CREATE TYPE battle_result AS ENUM ('AttackerWin', 'DefenderWin', 'Draw');

CREATE TABLE Elements
(
    ElementId SERIAL PRIMARY KEY,
    Name      VARCHAR(50) NOT NULL
);

CREATE TABLE Cards
(
    CardId    SERIAL PRIMARY KEY,
    Name      VARCHAR(50) UNIQUE NOT NULL,
    ElementId INT                NOT NULL,
    FOREIGN KEY (ElementId) REFERENCES Elements (ElementId) ON DELETE RESTRICT
);


CREATE TABLE Users
(
    UserId    SERIAL PRIMARY KEY,
    Username  VARCHAR(50) UNIQUE     NOT NULL,
    Password  VARCHAR(500)            NOT NULL,
    Name      VARCHAR(50),
    BIO       TEXT,
    IMAGE     VARCHAR(50),
    ELO       INT                             DEFAULT 1000 CHECK (ELO >= 1000),
    Coins     INT CHECK (Coins >= 0) NOT NULL DEFAULT 20,
    IsAdmin   BOOLEAN                NOT NULL DEFAULT FALSE,
    DeletedOn TIMESTAMP
);


CREATE INDEX idx_users_username ON Users (Username);


CREATE TABLE UserCards
(
    UserCardId UUID PRIMARY KEY                                        DEFAULT gen_random_uuid(),
    UserId     INT                                            NOT NULL,
    CardId     INT                                            NOT NULL,
    Damage     INT CHECK (Damage >= 0) CHECK ( Damage <= 100) NOT NULL,
    AcquiredOn TIMESTAMP                                      NOT NULL DEFAULT NOW(),
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (CardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);


CREATE INDEX idx_user_cards_userid ON UserCards (UserId);

CREATE TABLE Decks
(
    DeckId      SERIAL PRIMARY KEY,
    UserId      int  NOT NULL,
    UserCardOneId UUID NOT NULL,
    UserCardTwoId UUID NOT NULL,
    UserCardThreeId UUID NOT NULL,
    UserCardFourId UUID NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (UserCardOneId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT,
    FOREIGN KEY (UserCardTwoId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT,
    FOREIGN KEY (UserCardThreeId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT,
    FOREIGN KEY (UserCardFourId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT
);

CREATE INDEX idx_deck_userid ON Decks (UserId);

CREATE TABLE ActiveDecks
(
    UserId INT NOT NULL,
    DeckId INT NOT NULL,
    PRIMARY KEY (UserId, DeckId),
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (DeckId) REFERENCES Decks (DeckId) ON DELETE RESTRICT
);

CREATE TABLE Battles
(
    BattleId           SERIAL PRIMARY KEY,
    PlayerOneId          INT           NOT NULL,
    PlayerTwoId        INT           NOT NULL,
    Result             battle_result NOT NULL,
    PlayerOneELOChange INT           NOT NULL,
    PlayerTwoELOChange INT           NOT NULL,
    HappenedOn         TIMESTAMP     NOT NULL DEFAULT NOW(),
    FOREIGN KEY (PlayerOneId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (PlayerTwoId) REFERENCES Users (UserId) ON DELETE RESTRICT
);

CREATE TABLE BattleDecks
(
    BattleId INT NOT NULL,
    UserId   INT NOT NULL,
    CardOneId  INT NOT NULL,
    CardTwoId  INT NOT NULL,
    CardThreeId  INT NOT NULL,
    CardFourId  INT NOT NULL,
    PRIMARY KEY (BattleId, UserId),
    FOREIGN KEY (BattleId) REFERENCES Battles (BattleId) ON DELETE RESTRICT,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (CardOneId) REFERENCES Cards (CardId) ON DELETE RESTRICT,
    FOREIGN KEY (CardTwoId) REFERENCES Cards (CardId) ON DELETE RESTRICT,
    FOREIGN KEY (CardThreeId) REFERENCES Cards (CardId) ON DELETE RESTRICT,
    FOREIGN KEY (CardFourId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);

CREATE TABLE CardPackages
(
    CardPackageId SERIAL PRIMARY KEY,
    Name          VARCHAR(50) NOT NULL,
    CreatedBy     INT         NOT NULL,
    CreatedOn     TIMESTAMP   NOT NULL DEFAULT NOW(),
    BoughtBy      INT,
    BoughtOn      TIMESTAMP,
    FOREIGN KEY (CreatedBy) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (BoughtBy) REFERENCES Users (UserId) ON DELETE RESTRICT
);

CREATE INDEX idx_card_packages_createdby ON CardPackages (CreatedBy);
CREATE INDEX idx_card_packages_boughtby ON CardPackages (BoughtBy);

CREATE TABLE CardPackageContents
(
    CardPackageId  INT  NOT NULL,
    AcquiredCardId UUID NOT NULL DEFAULT gen_random_uuid(),
    CardId         INT  NOT NULL,
    Damage         INT  NOT NULL,
    PRIMARY KEY (CardPackageId, AcquiredCardId),
    FOREIGN KEY (CardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);

CREATE Table Deals
(
    DealId           SERIAL PRIMARY KEY,
    OfferingUserId   INT       NOT NULL,
    OfferingCardId   INT       NOT NULL,
    RespondingUserId INT,
    RespondingCardId INT,
    RequiredCardType VARCHAR(50) NOT NULL,
    MinimalDamage    INT CHECK ( MinimalDamage >= 0) CHECK ( MinimalDamage <= 100),
    FOREIGN KEY (OfferingUserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (OfferingCardId) REFERENCES Cards (CardId) ON DELETE RESTRICT,
    FOREIGN KEY (RespondingUserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (RespondingCardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);

CREATE INDEX idx_deals_offeringuserid ON Deals (OfferingUserId);
CREATE INDEX idx_deals_respondinguserid ON Deals (RespondingUserId);