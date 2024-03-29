﻿// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Elements;

public partial class Track
{
    public enum TrackData
    {
        StanPodstawowy = 0,
        RejonManewrowy = 1,
        PrzebiegManewrowy = 2,
        OchronaPrzebiegu = 3,
        PrzebiegPociagowy = 4,
        ZwalnianyCzasowo = 5,
        Zajety = 6,
        PierwszyPrzejazd = 7,
        PotwierdzenieZerowania = 8,
        UszkodzenieKontroli = 9,
        BrakDanych = 10
    }
}