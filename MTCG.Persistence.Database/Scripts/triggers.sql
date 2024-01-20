CREATE OR REPLACE FUNCTION update_attacker_elo()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE Users
    SET ELO = GREATEST(ELO + NEW.playeroneelochange, 1000)
    WHERE UserId = NEW.playeroneid;

    UPDATE Users
    SET ELO = GREATEST(ELO + NEW.playertwoelochange, 1000)
    WHERE UserId = NEW.playertwoid;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_attacker_elo
    AFTER INSERT
    ON Battles
    FOR EACH ROW
EXECUTE FUNCTION update_attacker_elo();
