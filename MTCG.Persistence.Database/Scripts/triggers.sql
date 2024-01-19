CREATE OR REPLACE FUNCTION update_attacker_elo()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE Users
    SET ELO = ELO + NEW.playeroneelochange
    WHERE UserId = NEW.playeroneid;

    UPDATE Users
    SET ELO = ELO + NEW.playertwoelochange
    WHERE UserId = NEW.playertwoid;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_attacker_elo
    AFTER INSERT
    ON Battles
    FOR EACH ROW
EXECUTE FUNCTION update_attacker_elo();
