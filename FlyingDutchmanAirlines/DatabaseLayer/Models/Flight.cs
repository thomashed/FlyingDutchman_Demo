﻿using System;
using System.Collections.Generic;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public class Flight
{
    public int FlightNumber { get; set; }

    public int Origin { get; set; }

    public int Destination { get; set; }

    public ICollection<Booking> Bookings { get; } = new List<Booking>();

    public Airport DestinationNavigation { get; set; } = null!;

    public Airport OriginNavigation { get; set; } = null!;
}
