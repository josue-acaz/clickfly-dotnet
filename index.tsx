import React, {useEffect, useState} from "react";
import {CommonActions} from "@react-navigation/native";
import {ScrollView} from "react-native";
import {EnumBookingStatus} from "../../../constants";

// types
import {StackScreenProps} from "@react-navigation/stack";

// models
import Booking from "../../../models/Booking";
import PaginationFilter from "../../../viewModels/PaginationFilter";

// services
import BookingService from "../../../services/booking.service";

// components
import ReservationItem from "./ReservationItem";
import LoadingSpinner from "../../../components/spinners/Loading";
import Overlay from "../../../components/Overlay";
import AwaitPayDialog from "./AwaitPayDialog";

// styles
import {
    ReservationsView,
    NoBookingsView,
    TicketImage,
    NoBookingsTitle,
    NoBookingsActionBtn,
    NoBookingsDescription,
    NoBookingsActionBtnText,
    ListReservationsView,
} from "./styles";
import {
    View, 
    ViewContainer, 
    Title, 
    Subtitle,
} from "../../../design";

// images
import ticket from "../../../assets/img/ticket.png";

export default function Reservations(props: StackScreenProps<any>)
{
    const {navigation} = props;

    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(true);
    const [rows, setRows] = useState<Array<Booking>>([]);
    const [pagination, setPagination] = useState<PaginationFilter>(new PaginationFilter());

    function toggleOpen()
    {
        setOpen(!open);
    }

    function handleChangePagination(key: string, value: any) {
        setPagination(pagination => ({ ...pagination, [key]: value }));
    }

    const bookingService = new BookingService();

    async function index()
    {
        try {
            const bookings = await bookingService.pagination(pagination);
            const {total_records, total_pages, data} = bookings;

            handleChangePagination("total_pages", total_pages);
            handleChangePagination("total_records", total_records);

            setRows(data);
            setLoading(false);
        } catch (error) {
            setLoading(false);
        }
    }

    useEffect(() => {
        const unsubscribe = navigation.addListener("focus", () => {
            // The screen is focused
            // Call any action
            index();
        });
    
        // Return the function to unsubscribe from the event so it gets removed on unmount
        return unsubscribe;
    }, [navigation]);

    function handleFindFlight()
    {
        navigation.dispatch(
            CommonActions.reset({
              index: 0,
              routes: [
                {
                  name: "Voar",
                  params: {screen: "OverviewFlights", params: {refresh: true}},
                },
              ],
            }),
        );
    }

    function handleSelected(booking: Booking)
    {
        const booking_id = booking.id;
        const flight_segment_id = booking.flight_segment_id;
        const status = booking.status[0].type;

        if(status === EnumBookingStatus.PENDING)
        {
            toggleOpen();
        }
        else if(status === EnumBookingStatus.APPROVED)
        {
            navigation.navigate("Tickets", {booking_id, flight_segment_id});
        }
    }

    return(
        <React.Fragment>
            <View>
                <ViewContainer>
                    {loading ? <LoadingSpinner /> : (
                        <ReservationsView>
                            {rows.length > 0 ? (
                                <ScrollView showsVerticalScrollIndicator={false}>
                                    <Title>Minhas viagens</Title>
                                    <Subtitle>Suas reservas aparecem aqui</Subtitle>
                                    <ListReservationsView>
                                        {rows.map(booking => (
                                            <ReservationItem booking={booking} onSelect={handleSelected} />
                                        ))}
                                    </ListReservationsView>
                                </ScrollView>
                            ) : (
                                <React.Fragment>
                                    <Title>Minhas viagens</Title>
                                    <Subtitle>Suas reservas aparecem aqui</Subtitle>
                                    <NoBookingsView>
                                        <TicketImage style={{resizeMode: "contain"}} source={ticket} />
                                        <NoBookingsDescription>
                                            <NoBookingsTitle>Você não possui viagens</NoBookingsTitle>
                                            <NoBookingsActionBtn onPress={handleFindFlight}>
                                                <NoBookingsActionBtnText>Procure um voo</NoBookingsActionBtnText>
                                            </NoBookingsActionBtn>
                                        </NoBookingsDescription>
                                    </NoBookingsView>
                                </React.Fragment>
                            )}
                        </ReservationsView>
                    )}
                </ViewContainer>
            </View>
            <Overlay open={open} onClose={toggleOpen}>
                <AwaitPayDialog onDimiss={toggleOpen} />
            </Overlay>
        </React.Fragment>
    );
}