﻿using MTCG.Persistance.Repositories;

namespace MTCG.Services.Cards.cards;

public interface CardsService
{

    ValueTask<IEnumerable<CardDto>> GetAllCards();

}