DROP TABLE IF EXISTS PackageContents;
DROP TABLE IF EXISTS TradingDeals;
DROP TABLE IF EXISTS BattleDeckContents;
DROP TABLE IF EXISTS BattleDecks;
DROP TABLE IF EXISTS Battles;
DROP TABLE IF EXISTS DeckContents;
DROP TABLE IF EXISTS Decks;
DROP TABLE IF EXISTS UserCards;
DROP TABLE IF EXISTS Packages;
DROP TABLE IF EXISTS Cards;
DROP TABLE IF EXISTS Elements;
DROP TABLE IF EXISTS Users;


DROP TYPE IF EXISTS battle_result;

CREATE TYPE battle_result AS ENUM ('PlayerOneWin', 'PlayerTwoWin', 'Draw');

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
    Password  VARCHAR(500)           NOT NULL,
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
    UserCardId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    UserId     INT                                            NOT NULL,
    CardId     INT                                            NOT NULL,
    Damage     INT CHECK (Damage >= 1) CHECK ( Damage <= 100) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (CardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);


CREATE INDEX idx_user_cards_userid ON UserCards (UserId);

CREATE TABLE Decks
(
    DeckId      SERIAL,
    UserId      int     NOT NULL,
    Description VARCHAR(50),
    IsActive    BOOLEAN NOT NULL DEFAULT FALSE,
    PRIMARY KEY (DeckId),
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT
);

CREATE INDEX idx_deck_userid ON Decks (UserId);

CREATE TABLE DeckContents
(
    DeckId     INT  NOT NULL,
    UserCardId UUID NOT NULL,
    PRIMARY KEY (DeckId, UserCardId),
    FOREIGN KEY (DeckId) REFERENCES Decks (DeckId) ON DELETE RESTRICT,
    FOREIGN KEY (UserCardId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT
);

CREATE TABLE Battles
(
    BattleId           SERIAL PRIMARY KEY,
    PlayerOneId        INT           NOT NULL,
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
    BattleDeckId SERIAL PRIMARY KEY,
    BattleId     INT NOT NULL,
    UserId       INT NOT NULL,
    FOREIGN KEY (BattleId) REFERENCES Battles (BattleId) ON DELETE RESTRICT,
    FOREIGN KEY (UserId) REFERENCES Users (UserId) ON DELETE RESTRICT
);

CREATE INDEX idx_battle_decks_userid ON BattleDecks (UserId);

CREATE TABLE BattleDeckContents
(
    BattleDeckContentId SERIAL PRIMARY KEY,
    BattleDeckId        INT                                            NOT NULL,
    CardId              INT                                            NOT NULL,
    Damage              INT CHECK (Damage >= 1) CHECK ( Damage <= 100) NOT NULL,
    FOREIGN KEY (BattleDeckId) REFERENCES BattleDecks (BattleDeckId) ON DELETE RESTRICT,
    FOREIGN KEY (CardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);

CREATE INDEX idx_battle_decks_battleid ON BattleDecks (BattleId);


CREATE TABLE Packages
(
    PackageId    SERIAL PRIMARY KEY,
    Name         VARCHAR(50) NOT NULL,
    CreatedById  INT         NOT NULL,
    CreatedOn    TIMESTAMP   NOT NULL DEFAULT NOW(),
    AcquiredById INT,
    AcquiredOn   TIMESTAMP,
    FOREIGN KEY (CreatedById) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (AcquiredById) REFERENCES Users (UserId) ON DELETE RESTRICT
);

CREATE INDEX idx_card_packages_createdby ON Packages (CreatedById);
CREATE INDEX idx_card_packages_acquiredby ON Packages (AcquiredById);

CREATE TABLE PackageContents
(
    PackageId     INT  NOT NULL,
    PackageCardId UUID NOT NULL UNIQUE DEFAULT gen_random_uuid(),
    CardId        INT  NOT NULL,
    Damage        INT  NOT NULL,
    PRIMARY KEY (PackageId, PackageCardId),
    FOREIGN KEY (CardId) REFERENCES Cards (CardId) ON DELETE RESTRICT
);

CREATE Table TradingDeals
(
    TradingDealId         UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    OfferingUserId        INT         NOT NULL,
    OfferingUserCardId    UUID        NOT NULL,
    RespondingUserId      INT,
    RespondingUserCardId  UUID,
    RequiredCardType      VARCHAR(50) NOT NULL,
    RequiredMinimumDamage INT CHECK ( RequiredMinimumDamage >= 1) CHECK ( RequiredMinimumDamage <= 100),
    FOREIGN KEY (OfferingUserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (OfferingUserCardId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT,
    FOREIGN KEY (RespondingUserId) REFERENCES Users (UserId) ON DELETE RESTRICT,
    FOREIGN KEY (RespondingUserCardId) REFERENCES UserCards (UserCardId) ON DELETE RESTRICT
);

CREATE INDEX idx_deals_offeringuserid ON TradingDeals (OfferingUserId);
CREATE INDEX idx_deals_respondinguserid ON TradingDeals (RespondingUserId);